using PluginAPI.Enums;
using PlayerRoles;
using PluginAPI.Core;
using PluginAPI.Core.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using MapGeneration.Distributors;
using UnityEngine;
using MEC;
using NorthwoodLib.Pools;
using PlayerRoles.PlayableScps.Scp079;
using PlayerRoles.PlayableScps.Scp106;
using Object = UnityEngine.Object;

namespace ScpsInfoDisplay
{
    public class EventHandlers
    {
        private CoroutineHandle _displayCoroutine;
        private List<Scp079Generator> _generators = new List<Scp079Generator>();
        
        [PluginEvent(ServerEventType.WaitingForPlayers)]
        public void OnWaitingForPlayers()
        {
            _generators = Object.FindObjectsOfType<Scp079Generator>().ToList();
        }

        [PluginEvent(ServerEventType.RoundStart)]
        public void OnRoundStart()
        {
            if (_displayCoroutine.IsRunning)
                Timing.KillCoroutines(_displayCoroutine);

            _displayCoroutine = Timing.RunCoroutine(ShowDisplay());
        }

        private IEnumerator<float> ShowDisplay()
        {
            while (!RoundSummary.singleton._roundEnded)
            {
                yield return Timing.WaitForSeconds(1f);
                try
                {
                    foreach (var player in Player.GetPlayers().Where(p => p != null && (ScpsInfoDisplay.Singleton.Config.DisplayStrings.ContainsKey(p.Role) || ScpsInfoDisplay.Singleton.Config.CustomRolesIntegrations.Keys.Any(key => p.TemporaryData.Contains(key)))))
                    {
                        var builder = StringBuilderPool.Shared.Rent($"<align={ScpsInfoDisplay.Singleton.Config.TextAlignment.ToString().ToLower()}>");
                        
                        foreach (var integration in ScpsInfoDisplay.Singleton.Config.CustomRolesIntegrations)
                        {
                            builder.Append(Player.GetPlayers().Where(p => p?.TemporaryData.Contains(integration.Key) == true).Aggregate(builder.ToString(), (current, any) => current + (player == any && ScpsInfoDisplay.Singleton.Config.MarkPlayerInList ? ScpsInfoDisplay.Singleton.Config.PlayersMarker + " " : "") + ProcessStringVariables(integration.Value, player, any) + "\n"));
                        }

                        foreach (var scp in Player.GetPlayers().Where(p => p?.Role.GetTeam() == Team.SCPs && ScpsInfoDisplay.Singleton.Config.DisplayStrings.ContainsKey(p.Role)))
                        {
                            builder.Append((player == scp && ScpsInfoDisplay.Singleton.Config.MarkPlayerInList ? ScpsInfoDisplay.Singleton.Config.PlayersMarker + " " : "") + ProcessStringVariables(ScpsInfoDisplay.Singleton.Config.DisplayStrings[scp.Role], player, scp) + "\n");
                        }

                        builder.Append($"<voffset={ScpsInfoDisplay.Singleton.Config.TextPositionOffset}em> </voffset></align>");
                        player.ReceiveHint(StringBuilderPool.Shared.ToStringReturn(builder), 1.25f);
                    }
                } 
                catch (Exception ex)
                {
                    Log.Error(ex.ToString());
                }
            }
        }

        private string ProcessStringVariables(string raw, Player observer, Player target)
        {
            switch (target.ReferenceHub.roleManager.CurrentRole)
            {
                case Scp079Role scp079:
                    scp079.SubroutineModule.TryGetSubroutine(out Scp079AuxManager aux);
                    scp079.SubroutineModule.TryGetSubroutine(out Scp079TierManager tier);
                    raw = raw
                        .Replace("%079energy%", Math.Floor(aux.CurrentAux).ToString())
                        .Replace("%079level%", tier.AccessTierLevel.ToString())
                        .Replace("%079experience", tier.RelativeExp.ToString());
                    break;
                case Scp106Role scp106:
                    scp106.SubroutineModule.TryGetSubroutine(out Scp106Vigor vigor);
                    raw = raw.Replace("%106vigor%", Math.Floor(vigor.VigorAmount * 100).ToString());
                    break;

            }

            return raw
                .Replace("%healthpercent%", Math.Floor(target.Health / target.MaxHealth * 100).ToString())
                .Replace("%health%", Math.Floor(target.Health).ToString())
                .Replace("%generators%", _generators.Count(gen => gen.Engaged).ToString())
                .Replace("%engaging%", _generators.Count(gen => gen.Activating) > 0 ? $" (+{_generators.Count(gen => gen.Activating)})" : "").Replace("%zombies%", Player.GetPlayers<Player>().Count(p => p.Role == RoleTypeId.Scp0492).ToString())
                .Replace("%distance%", target != observer ? Math.Floor(Vector3.Distance(observer.Position, target.Position)) + "m" : "");
        }
    }
}