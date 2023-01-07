using Exiled.API.Features;
using System;
using Server = Exiled.Events.Handlers.Server;

namespace ScpsInfoDisplay
{
    internal class ScpsInfoDisplay : Plugin<Config>
    {
        public override string Prefix => "scpsinfodisplay";
        public override string Name => "ScpsInfoDisplay";
        public override string Author => "bladuk.";
        public override Version Version { get; } = new Version(2, 0, 0);
        public override Version RequiredExiledVersion { get; } = new Version(6, 0, 0);
        public static ScpsInfoDisplay Singleton = new ScpsInfoDisplay();
        private EventHandlers _eventHandlers;

        public override void OnEnabled()
        {
            Singleton = this;
            _eventHandlers = new EventHandlers();

            ValidateConfigs();
            RegisterEvents();

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            UnregisterEvents();
            _eventHandlers = null;

            base.OnDisabled();
        }

        private void ValidateConfigs()
        {
            if (!string.Equals(Config.TextAlignment, "center", StringComparison.OrdinalIgnoreCase) && !string.Equals(Config.TextAlignment, "left", StringComparison.OrdinalIgnoreCase) && !string.Equals(Config.TextAlignment, "right", StringComparison.OrdinalIgnoreCase))
            {
                Log.Warn(Config.TextAlignment + " is an invalid value of text_alignment config. It will be replaced with the default value (right).");
                Config.TextAlignment = "right";
            }

            if (Config.MarkPlayerInList && string.IsNullOrEmpty(Config.PlayersMarker))
            {
                Log.Warn("Player's marker string is null or empty. It will be replaced with the default value (You -->).");
                Config.PlayersMarker = "<color=#D51D1D>You --></color>";
            }
        }

        private void RegisterEvents()
        {
            Server.ReloadedConfigs += ValidateConfigs;
            Server.RoundStarted += _eventHandlers.OnRoundStarted;
        }

        private void UnregisterEvents()
        {
            Server.ReloadedConfigs -= ValidateConfigs;
            Server.RoundStarted -= _eventHandlers.OnRoundStarted;
        }
    }
}