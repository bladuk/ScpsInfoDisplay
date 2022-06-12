using System;
using Exiled.API.Features;
using Server = Exiled.Events.Handlers.Server;
using Player = Exiled.Events.Handlers.Player;

namespace ScpsInfoDisplay
{
    internal class ScpsInfoDisplay : Plugin<Config>
    {
        public override string Prefix => "scpsinfodisplay";
        public override string Name => "ScpsInfoDisplay";
        public override string Author => "bladuk.";
        public override Version Version { get; } = new Version(1, 1, 5);
        public override Version RequiredExiledVersion { get; } = new Version(5, 2, 1);
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

        internal void ValidateConfigs()
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

            if (Config.TextPositionOffset < -10)
            {
                Log.Warn("Text position offset can't be less than -10.");
                Config.TextPositionOffset = -10;
            }

            if (Config.TextPositionOffset > 42)
            {
                Log.Warn("Text position offset can't be more than 42.");
                Config.TextPositionOffset = 42;
            }
        }

        private void RegisterEvents()
        {
            Server.ReloadedConfigs += _eventHandlers.OnReloadedConfigs;
            Server.RestartingRound += _eventHandlers.OnRoundRestart;
            Player.ChangingRole += _eventHandlers.OnPlayerChangingRole;
        }

        private void UnregisterEvents()
        {
            Server.ReloadedConfigs -= _eventHandlers.OnReloadedConfigs;
            Server.RestartingRound -= _eventHandlers.OnRoundRestart;
            Player.ChangingRole -= _eventHandlers.OnPlayerChangingRole;
        }
    }
}