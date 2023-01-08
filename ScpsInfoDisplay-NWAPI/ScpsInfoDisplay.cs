using System;
using PluginAPI.Core;
using PluginAPI.Core.Attributes;
using PluginAPI.Events;

namespace ScpsInfoDisplay
{
    internal sealed class ScpsInfoDisplay
    {
        public static ScpsInfoDisplay Singleton;

        [PluginConfig]
        public Config Config;

        [PluginEntryPoint("ScpsInfoDisplay", "2.0.0", "A plugin that displays information about SCPs in player's team.", "bladuk.")]
        private void LoadPlugin()
        {
            Singleton = this;
            
            #region Config validation
            if (!string.Equals(Config.TextAlignment, "center", StringComparison.OrdinalIgnoreCase) && !string.Equals(Config.TextAlignment, "left", StringComparison.OrdinalIgnoreCase) && !string.Equals(Config.TextAlignment, "right", StringComparison.OrdinalIgnoreCase))
            {
                Log.Warning(Config.TextAlignment + " is an invalid value of text_alignment config. It will be replaced with the default value (right).");
                Config.TextAlignment = "right";
            }

            if (Config.MarkPlayerInList && string.IsNullOrEmpty(Config.PlayersMarker))
            {
                Log.Warning("Player's marker string is null or empty. It will be replaced with the default value (You -->).");
                Config.PlayersMarker = "<color=#D51D1D>You --></color>";
            }
            #endregion
            
            EventManager.RegisterEvents<EventHandlers>(this);
        }
    }
}