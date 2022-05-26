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
                else if ((ev.Player.Role.Team != Team.SCP && ev.NewRole.GetTeam() == Team.SCP && ScpsInfoDisplay.Singleton.Config.DisplayStrings.ContainsKey(ev.NewRole)) || ScpsInfoDisplay.Singleton.Config.CustomRolesIntegrations.Keys.Any(key => ev.Player.SessionVariables.ContainsKey(key)))
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
                string text = string.Empty;
                sbyte integrations = 0;
                foreach (Player scp in Player.List.Where(p => p.Role.Team == Team.SCP))
                {
                    text += "<align=right>" + ScpsInfoDisplay.Singleton.Config.DisplayStrings[scp.Role.Type].Replace("%arhealth%", scp.ArtificialHealth > 0 ? scp.ArtificialHealth.ToString() : "").Replace("%healthpercent%", Math.Round((Mathf.Clamp01(scp.Health / (float)scp.MaxHealth) * 100), 0).ToString()).Replace("%health%", Math.Round(scp.Health, 0).ToString()).Replace("%generators%", Generator.List.Count(gen => gen.IsEngaged).ToString()).Replace("%engaging%", Generator.List.Count(gen => gen.IsActivating) > 0 ? $" (+{Generator.List.Count(gen => gen.IsActivating)})" : "") + "</align>\n";
                }

                foreach (KeyValuePair<string, string> integration in ScpsInfoDisplay.Singleton.Config.CustomRolesIntegrations)
                {
                    foreach (Player any in Player.List)
                    {
                        if (any.SessionVariables.ContainsKey(integration.Key))
                        {
                            text += "<align=right>" + integration.Value.Replace("%arhealth%", any.ArtificialHealth > 0 ? any.ArtificialHealth.ToString() : "").Replace("%healthpercent%", Math.Round((Mathf.Clamp01(any.Health / (float)any.MaxHealth) * 100), 0).ToString()).Replace("%health%", Math.Round(any.Health, 0).ToString()).Replace("%generators%", Generator.List.Count(gen => gen.IsEngaged).ToString()).Replace("%engaging%", Generator.List.Count(gen => gen.IsActivating) > 0 ? $" (+{Generator.List.Count(gen => gen.IsActivating)})" : "") + "</align>\n";
                            integrations++;
                        }
                    }
                }
                text += new string('\n', 31 - Player.List.Count(p => p.Role.Team == Team.SCP) - integrations);
                player.ShowHint(text, 1f);
            }
        }
    }
}