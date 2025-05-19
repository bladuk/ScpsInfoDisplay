﻿using Exiled.API.Interfaces;
using PlayerRoles;
using ScpsInfoDisplay.Enums;
using System.Collections.Generic;
using System.ComponentModel;

namespace ScpsInfoDisplay
{
    public class Config : IConfig
    {
        [Description("Is the plugin enabled?")]
        public bool IsEnabled { get; set; } = true;
        
        [Description("Display strings. Format: Role, display string.")]
        public Dictionary<RoleTypeId, string> DisplayStrings { get; set; } = new Dictionary<RoleTypeId, string>()
        {
            { RoleTypeId.Scp049, "<color=%roleColor%>SCP-049 [%healthpercent%% Zombies: %zombies%] %distance%</color>" },
            { RoleTypeId.Scp079, "<color=%roleColor%>SCP-079 [%generators%%engaging%/3] Lvl: %079level% Exp: %079experience% Energy: %079energy%</color>" },
            { RoleTypeId.Scp096, "<color=%roleColor%>SCP-096 [%healthpercent%%] %distance%</color>" },
            { RoleTypeId.Scp106, "<color=%roleColor%>SCP-106 [%healthpercent%%] Vigor: %106vigor%% %distance%</color>" },
            { RoleTypeId.Scp173, "<color=%roleColor%>SCP-173 [%healthpercent%%] %distance%</color>" },
            { RoleTypeId.Scp939, "<color=%roleColor%>SCP-939 [%healthpercent%%] %distance%</color>" },
            { RoleTypeId.Scp3114, "<color=%roleColor%>SCP-3114 [%healthpercent%%] %distance%</color>" }
        };
        
        [Description("Custom roles integrations. Format: SessionVariable that marks that the player belongs to that role, display string.")]
        public Dictionary<string, string> CustomRolesIntegrations { get; set; } = new Dictionary<string, string>();
        
        [Description("Display string alignment. Possible values: Left, Center, Right.")]
        public TextAlignment TextAlignment { get; set; } = TextAlignment.Right;
        
        [Description("Display text position offset.")]
        public sbyte TextPositionOffset { get; set; } = 30;
        
        [Description("The player seeing the list will be highlighted with the special marker to the left. Leave it empty if disabled.")]
        public string PlayersMarker { get; set; } = "<color=%roleColor%>You --></color>";
        
        [Description("Display debug messages in server console?")]
        public bool Debug { get; set; } = false;
    }
}