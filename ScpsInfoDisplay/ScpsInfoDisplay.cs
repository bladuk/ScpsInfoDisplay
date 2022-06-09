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
        public override Version Version { get; } = new Version(1, 1, 4);
        public override Version RequiredExiledVersion { get; } = new Version(5, 2, 1);
        public static ScpsInfoDisplay Singleton = new ScpsInfoDisplay();
        private EventHandlers _eventHandlers;

        public override void OnEnabled()
        {
            Singleton = this;
            _eventHandlers = new EventHandlers();
            RegisterEvents();

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            UnregisterEvents();

            base.OnDisabled();
        }

        private void RegisterEvents()
        {
            Server.WaitingForPlayers += _eventHandlers.OnWaitingForPlayers;
            Server.RestartingRound += _eventHandlers.OnRoundRestart;
            Player.ChangingRole += _eventHandlers.OnPlayerChangingRole;
        }

        private void UnregisterEvents()
        {
            Server.WaitingForPlayers -= _eventHandlers.OnWaitingForPlayers;
            Server.RestartingRound -= _eventHandlers.OnRoundRestart;
            Player.ChangingRole -= _eventHandlers.OnPlayerChangingRole;
        }
    }
}