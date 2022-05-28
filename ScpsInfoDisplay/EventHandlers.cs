using System;
using System.Collections.Generic;
using System.Linq;
using Exiled.API.Extensions;
using Exiled.API.Features;
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
            if (ScpsInfoDisplay.Singleton.Config.TextAlignment.ToLower() != "center" && ScpsInfoDisplay.Singleton.Config.TextAlignment.ToLower() != "left" && ScpsInfoDisplay.Singleton.Config.TextAlignment.ToLower() != "right")
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
            foreach (KeyValuePair<Player, CoroutineHandle> kvp in _allDisplays)
            {
                if (kvp.Value.IsRunning)
                    Timing.KillCoroutines(kvp.Value);
                _allDisplays.Remove(kvp.Key);
            }
            _allDisplays.Clear();
        }

        public void OnPlayerChangingRole(ChangingRoleEventArgs ev)
        {
            if (ev.Player != null)
            {
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
        }

        private IEnumerator<float> _showDisplay(Player player)
        {
            for (; ; )
            {
                yield return Timing.WaitForSeconds(0.5f);
                try
                {
                    string text = string.Empty;
                    foreach (KeyValuePair<string, string> integration in ScpsInfoDisplay.Singleton.Config.CustomRolesIntegrations)
                    {
                        foreach (Player any in Player.List)
                        {
                            if (any != null && any.SessionVariables.ContainsKey(integration.Key))
                            {
                                text += $"<align={ScpsInfoDisplay.Singleton.Config.TextAlignment}>" + (player == any && ScpsInfoDisplay.Singleton.Config.MarkPlayerInList ? ScpsInfoDisplay.Singleton.Config.PlayersMarker + " " : "") + integration.Value.Replace("%arhealth%", any.ArtificialHealth > 0 ? any.ArtificialHealth.ToString() : "").Replace("%healthpercent%", Math.Round((Mathf.Clamp01(any.Health / (float)any.MaxHealth) * 100), 0).ToString()).Replace("%health%", Math.Round(any.Health, 0).ToString()).Replace("%generators%", Generator.List.Count(gen => gen.IsEngaged).ToString()).Replace("%engaging%", Generator.List.Count(gen => gen.IsActivating) > 0 ? $" (+{Generator.List.Count(gen => gen.IsActivating)})" : "").Replace("%zombies%", Player.List.Count(p => p.Role.Type == RoleType.Scp0492).ToString()).Replace("%distance%", any != player ? Math.Round(Vector3.Distance(player.Position, any.Position), 0).ToString() + "m" : "") + "</align>\n";
                            }
                        }
                    }

                    foreach (Player scp in Player.List.Where(p => p.Role.Team == Team.SCP && ScpsInfoDisplay.Singleton.Config.DisplayStrings.ContainsKey(p.Role.Type)))
                    {
                        if (scp != null)
                        {
                            text += $"<align={ScpsInfoDisplay.Singleton.Config.TextAlignment}>" + (player == scp && ScpsInfoDisplay.Singleton.Config.MarkPlayerInList ? ScpsInfoDisplay.Singleton.Config.PlayersMarker + " " : "") + ScpsInfoDisplay.Singleton.Config.DisplayStrings[scp.Role.Type].Replace("%arhealth%", scp.ArtificialHealth > 0 ? scp.ArtificialHealth.ToString() : "").Replace("%healthpercent%", Math.Round((Mathf.Clamp01(scp.Health / (float)scp.MaxHealth) * 100), 0).ToString()).Replace("%health%", Math.Round(scp.Health, 0).ToString()).Replace("%generators%", Generator.List.Count(gen => gen.IsEngaged).ToString()).Replace("%engaging%", Generator.List.Count(gen => gen.IsActivating) > 0 ? $" (+{Generator.List.Count(gen => gen.IsActivating)})" : "").Replace("%zombies%", Player.List.Count(p => p.Role.Type == RoleType.Scp0492).ToString()).Replace("%distance%", scp != player ? Math.Round(Vector3.Distance(player.Position, scp.Position), 0).ToString() + "m" : "") + "</align>\n";
                        }
                    }

                    if (ScpsInfoDisplay.Singleton.Config.TextPositionOffset >= 0 || (ScpsInfoDisplay.Singleton.Config.TextPositionOffset < 0 && (ScpsInfoDisplay.Singleton.Config.TextPositionOffset + text.Count(t => t == '\n')) > 0))
                        text += new string('\n', ScpsInfoDisplay.Singleton.Config.TextPositionOffset >= 0 ? Math.Abs(ScpsInfoDisplay.Singleton.Config.TextPositionOffset - text.Count(t => t == '\n')) : ScpsInfoDisplay.Singleton.Config.TextPositionOffset + text.Count(t => t == '\n'));
                    else if (ScpsInfoDisplay.Singleton.Config.TextPositionOffset < 0)
                        text = text.Insert(0, new string('\n', Math.Abs(ScpsInfoDisplay.Singleton.Config.TextPositionOffset + text.Count(t => t == '\n'))));
                    player.ShowHint(text, 1f);
                } 
                catch (Exception ex)
                {
                    Log.Error(ex);
                }
            }
        }
    }
}