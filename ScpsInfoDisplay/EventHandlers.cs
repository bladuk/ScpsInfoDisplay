using Exiled.API.Features;
using Exiled.API.Features.Roles;
using MEC;
using NorthwoodLib.Pools;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Player = Exiled.API.Features.Player;

namespace ScpsInfoDisplay
{
    internal class EventHandlers
    {
        private CoroutineHandle _displayCoroutine;
        
        internal void OnRoundStarted()
        {
            if (_displayCoroutine.IsRunning)
                Timing.KillCoroutines(_displayCoroutine);

            _displayCoroutine = Timing.RunCoroutine(ShowDisplay());
        }

        private IEnumerator<float> ShowDisplay()
        {
            while (Round.InProgress)
            {
                yield return Timing.WaitForSeconds(1f);
                try
                {
                    foreach (var player in Player.List.Where(p => p != null && (ScpsInfoDisplay.Singleton.Config.DisplayStrings.ContainsKey(p.Role.Type) || ScpsInfoDisplay.Singleton.Config.CustomRolesIntegrations.Keys.Any(key => p.SessionVariables.ContainsKey(key)))))
                    {
                        var builder = StringBuilderPool.Shared.Rent($"<align={ScpsInfoDisplay.Singleton.Config.TextAlignment.ToString().ToLower()}>");
                        
                        foreach (var integration in ScpsInfoDisplay.Singleton.Config.CustomRolesIntegrations)
                        {
                            builder.Append(Player.List.Where(p => p?.SessionVariables.ContainsKey(integration.Key) == true).Aggregate(builder.ToString(), (current, any) => current + (player == any ? ScpsInfoDisplay.Singleton.Config.PlayersMarker : "") + ProcessStringVariables(integration.Value, player, any))).Append('\n');
                        }

                        foreach (var scp in Player.List.Where(p => p?.Role.Team == Team.SCPs && ScpsInfoDisplay.Singleton.Config.DisplayStrings.ContainsKey(p.Role.Type)))
                        {
                            builder.Append((scp == player ? ScpsInfoDisplay.Singleton.Config.PlayersMarker : "") + ProcessStringVariables(ScpsInfoDisplay.Singleton.Config.DisplayStrings[scp.Role.Type], player, scp)).Append('\n');
                        }

                        builder.Append($"<voffset={ScpsInfoDisplay.Singleton.Config.TextPositionOffset}em> </voffset></align>");
                        player.ShowHint(StringBuilderPool.Shared.ToStringReturn(builder), 1.25f);
                    }
                } 
                catch (Exception ex)
                {
                    Log.Error(ex);
                }
            }
        }

        private string ProcessStringVariables(string raw, Player observer, Player target) => raw
            .Replace("%arhealth%", target.ArtificialHealth > 0 ? target.ArtificialHealth.ToString() : "")
            .Replace("%healthpercent%", Math.Floor(target.Health / target.MaxHealth * 100).ToString())
            .Replace("%health%", Math.Floor(target.Health).ToString())
            .Replace("%generators%", Generator.List.Count(gen => gen.IsEngaged).ToString())
            .Replace("%engaging%", Generator.List.Count(gen => gen.IsActivating) > 0 ? $" (+{Generator.List.Count(gen => gen.IsActivating)})" : "")
            .Replace("%distance%", target != observer ? Math.Floor(Vector3.Distance(observer.Position, target.Position)) + "m" : "")
            .Replace("%zombies%", Player.List.Count(p => p.Role.Type == RoleTypeId.Scp0492).ToString())
            .Replace("%079level%", target.Role.Is(out Scp079Role scp079) ? scp079.Level.ToString() : "")
            .Replace("%079energy%", target.Role.Is(out Scp079Role _) ? Math.Floor(scp079.Energy).ToString() : "")
            .Replace("%079experience%", target.Role.Is(out Scp079Role _) ? Math.Floor((double)scp079.Experience).ToString() : "")
            .Replace("%106vigor%", target.Role.Is(out Scp106Role scp106) ? Math.Floor(scp106.Vigor * 100).ToString() : "");
    }
}