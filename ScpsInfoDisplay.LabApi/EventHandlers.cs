using System;
using System.Collections.Generic;
using System.Linq;
using LabApi.Events.CustomHandlers;
using LabApi.Features.Wrappers;
using MEC;
using NorthwoodLib.Pools;
using PlayerRoles;
using PlayerRoles.PlayableScps.Scp079;
using PlayerRoles.PlayableScps.Scp106;
using UnityEngine;
using Logger = LabApi.Features.Console.Logger;

namespace ScpsInfoDisplay.LabApi
{
    public sealed class EventHandlers : CustomEventsHandler 
    {
        private CoroutineHandle _displayCoroutine;
        
        public override void OnServerRoundStarted()
        {
            if (_displayCoroutine.IsRunning)
                Timing.KillCoroutines(_displayCoroutine);

            _displayCoroutine = Timing.RunCoroutine(ShowDisplay());
            
            base.OnServerRoundStarted();
        }
        
        private IEnumerator<float> ShowDisplay()
        {
            while (!Round.IsRoundEnded)
            {
                yield return Timing.WaitForSeconds(1f);
                try
                {
                    foreach (var player in Player.List.Where(p => p != null && ScpsInfoDisplay.Singleton.Config.DisplayStrings.ContainsKey(p.Role)))
                    {
                        var builder = StringBuilderPool.Shared.Rent($"<align={ScpsInfoDisplay.Singleton.Config.TextAlignment.ToString().ToLower()}>");
                
                        foreach (var scp in Player.List.Where(p => p?.Role.GetTeam() == Team.SCPs && ScpsInfoDisplay.Singleton.Config.DisplayStrings.ContainsKey(p.Role)))
                        {
                            builder.Append((player == scp && ScpsInfoDisplay.Singleton.Config.MarkPlayerInList ? ScpsInfoDisplay.Singleton.Config.PlayersMarker + " " : "") + ProcessStringVariables(ScpsInfoDisplay.Singleton.Config.DisplayStrings[scp.Role], player, scp) + "\n");
                        }

                        builder.Append($"<voffset={ScpsInfoDisplay.Singleton.Config.TextPositionOffset}em> </voffset></align>");
                        player.SendHint(StringBuilderPool.Shared.ToStringReturn(builder), 1.25f);
                    }
                } 
                catch (Exception ex)
                {
                    Logger.Error(ex.ToString());
                }
            }
        }

        private string ProcessStringVariables(string raw, Player observer, Player target)
        {
            switch (target.RoleBase)
            {
                case Scp079Role scp079:
                    scp079.SubroutineModule.TryGetSubroutine(out Scp079AuxManager aux);
                    scp079.SubroutineModule.TryGetSubroutine(out Scp079TierManager tier);
                    raw = raw
                        .Replace("%079energy%", Math.Floor(aux.CurrentAux).ToString())
                        .Replace("%079level%", tier.AccessTierLevel.ToString())
                        .Replace("%079experience%", tier.RelativeExp.ToString());
                    break;
                case Scp106Role scp106:
                    raw = raw.Replace("%106vigor%", Math.Floor(scp106.TargetStats.GetModule<VigorStat>().CurValue * 100).ToString());
                    break;
            }
            
            return raw
                .Replace("%healthpercent%", Math.Floor(target.Health / target.MaxHealth * 100).ToString())
                .Replace("%health%", Math.Floor(target.Health).ToString())
                .Replace("%generators%", Generator.List.Count(gen => gen.Engaged).ToString())
                .Replace("%engaging%", Generator.List.Count(gen => gen.Activating) > 0 ? $" (+{Generator.List.Count(gen => gen.Activating)})" : "").Replace("%zombies%", Player.List.Count(p => p.Role == RoleTypeId.Scp0492).ToString())
                .Replace("%distance%", target != observer ? Math.Floor(Vector3.Distance(observer.Position, target.Position)) + "m" : "")
                .Replace("%arhealth%", target.HumeShield > 0 ? target.HumeShield.ToString() : "")
                .Replace("%roleColor%", target.RoleBase.RoleColor.ToHex());
        }
    }
}