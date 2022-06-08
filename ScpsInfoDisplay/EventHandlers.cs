using System;
using System.Collections.Generic;
using System.Linq;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Roles;
using Exiled.Events.EventArgs;
using MEC;
using UnityEngine;

namespace ScpsInfoDisplay
{
    public class EventHandlers
    {
        private readonly Dictionary<Player, CoroutineHandle> _allDisplays = new Dictionary<Player, CoroutineHandle>();

        public void OnWaitingForPlayers()
        {
            if (!string.Equals(ScpsInfoDisplay.Singleton.Config.TextAlignment, "center", StringComparison.OrdinalIgnoreCase) && !string.Equals(ScpsInfoDisplay.Singleton.Config.TextAlignment, "left", StringComparison.OrdinalIgnoreCase) && !string.Equals(ScpsInfoDisplay.Singleton.Config.TextAlignment, "right", StringComparison.OrdinalIgnoreCase))
            {
                Log.Warn(ScpsInfoDisplay.Singleton.Config.TextAlignment + " is an invalid value of text_alignment config. It will be replaced with the default value (right).");
                ScpsInfoDisplay.Singleton.Config.TextAlignment = "right";
            }

            if (ScpsInfoDisplay.Singleton.Config.MarkPlayerInList && string.IsNullOrEmpty(ScpsInfoDisplay.Singleton.Config.PlayersMarker))
            {
                Log.Warn("Player's marker string is null or empty. It will be replaced with the default value (You -->).");
                ScpsInfoDisplay.Singleton.Config.PlayersMarker = "<color=#D51D1D>You --></color>";
            }

            if (ScpsInfoDisplay.Singleton.Config.TextPositionOffset < -10)
            {
                Log.Warn("Text position offset can't be less than -10.");
                ScpsInfoDisplay.Singleton.Config.TextPositionOffset = -10;
            }

            if (ScpsInfoDisplay.Singleton.Config.TextPositionOffset > 42)
            {
                Log.Warn("Text position offset can't be more than 42.");
                ScpsInfoDisplay.Singleton.Config.TextPositionOffset = 42;
            }
        }

        public void OnRoundRestart()
        {
            foreach (var display in _allDisplays.Where(display => display.Key != null && display.Value.IsRunning))
            {
                Timing.KillCoroutines(display.Value);
            }
            _allDisplays.Clear();
        }

        public void OnPlayerChangingRole(ChangingRoleEventArgs ev)
        {
            if (ev.Player == null) return;
            if (_allDisplays.ContainsKey(ev.Player) && !ScpsInfoDisplay.Singleton.Config.DisplayStrings.ContainsKey(ev.NewRole))
            {
                Timing.KillCoroutines(_allDisplays[ev.Player]);
                _allDisplays.Remove(ev.Player);
            }
            else if ((!_allDisplays.ContainsKey(ev.Player) && ev.NewRole.GetTeam() == Team.SCP && ScpsInfoDisplay.Singleton.Config.DisplayStrings.ContainsKey(ev.NewRole)) || ScpsInfoDisplay.Singleton.Config.CustomRolesIntegrations.Keys.Any(key => ev.Player.SessionVariables.ContainsKey(key)))
            {
                _allDisplays.Add(ev.Player, Timing.RunCoroutine(_showDisplay(ev.Player)));
            }
        }

        private IEnumerator<float> _showDisplay(Player player)
        {
            for (; ; )
            {
                yield return Timing.WaitForSeconds(0.5f);
                try
                {
                    if (player != null)
                    {
                        string text = string.Empty;
                        foreach (var integration in ScpsInfoDisplay.Singleton.Config.CustomRolesIntegrations)
                        {
                            text = Player.List.Where(p => p != null && p.SessionVariables.ContainsKey(integration.Key)).Aggregate(text, (current, any) => current + $"<align={ScpsInfoDisplay.Singleton.Config.TextAlignment}>" + (player == any && ScpsInfoDisplay.Singleton.Config.MarkPlayerInList ? ScpsInfoDisplay.Singleton.Config.PlayersMarker + " " : "") + ProcessStringVariables(integration.Value, player, any) + "</align>\n");
                        }

                        foreach (var scp in Player.List.Where(p => p != null && p.Role.Team == Team.SCP && ScpsInfoDisplay.Singleton.Config.DisplayStrings.ContainsKey(p.Role.Type)))
                        {
                            text += $"<align={ScpsInfoDisplay.Singleton.Config.TextAlignment}>" + (player == scp && ScpsInfoDisplay.Singleton.Config.MarkPlayerInList ? ScpsInfoDisplay.Singleton.Config.PlayersMarker + " " : "") + ProcessStringVariables(ScpsInfoDisplay.Singleton.Config.DisplayStrings[scp.Role.Type], player, scp) + "</align>\n";
                        }

                        if (ScpsInfoDisplay.Singleton.Config.TextPositionOffset >= 0 || (ScpsInfoDisplay.Singleton.Config.TextPositionOffset < 0 && (ScpsInfoDisplay.Singleton.Config.TextPositionOffset + text.Count(t => t == '\n')) > 0))
                            text += new string('\n', ScpsInfoDisplay.Singleton.Config.TextPositionOffset >= 0 ? Math.Abs(ScpsInfoDisplay.Singleton.Config.TextPositionOffset - text.Count(t => t == '\n')) : ScpsInfoDisplay.Singleton.Config.TextPositionOffset + text.Count(t => t == '\n'));
                        else if (ScpsInfoDisplay.Singleton.Config.TextPositionOffset < 0)
                            text = text.Insert(0, new string('\n', Math.Abs(ScpsInfoDisplay.Singleton.Config.TextPositionOffset + text.Count(t => t == '\n'))));
                        player.ShowHint(text, 1f);
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
            .Replace("%healthpercent%", Math.Floor(Mathf.Clamp01(target.Health / target.MaxHealth) * 100).ToString())
            .Replace("%health%", Math.Round(target.Health, 0).ToString())
            .Replace("%generators%", Generator.List.Count(gen => gen.IsEngaged).ToString())
            .Replace("%engaging%", Generator.List.Count(gen => gen.IsActivating) > 0 ? $" (+{Generator.List.Count(gen => gen.IsActivating)})" : "").Replace("%zombies%", Player.List.Count(p => p.Role.Type == RoleType.Scp0492).ToString())
            .Replace("%distance%", target != observer ? Math.Floor(Vector3.Distance(observer.Position, target.Position)) + "m" : "")
            .Replace("%079level%", target.Role.Is(out Scp079Role _) ? (target.Role.As<Scp079Role>().Level + 1).ToString() : "")
            .Replace("%079energy%", target.Role.Is(out Scp079Role _) ? Math.Floor(target.Role.As<Scp079Role>().Energy).ToString() : "")
            .Replace("%079experience%", target.Role.Is(out Scp079Role _) ? Math.Floor(target.Role.As<Scp079Role>().Experience).ToString() : "");
    }
}