using System.ComponentModel;
using System.Collections.Generic;
using PlayerRoles;

namespace ScpsInfoDisplay
{
    public class Config
    {
        [Description("Display strings. Format: Role, display string.")]
        public Dictionary<RoleTypeId, string> DisplayStrings { get; set; } = new Dictionary<RoleTypeId, string>()
        {
            { RoleTypeId.Scp106, "<color=#D51D1D>SCP-106 [%healthpercent%%] Vigor: %106vigor%% %distance%</color>" },
            { RoleTypeId.Scp049, "<color=#D51D1D>SCP-049 [%healthpercent%% Zombies: %zombies%] %distance%</color>" },
            { RoleTypeId.Scp079, "<color=#D51D1D>SCP-079 [%generators%%engaging%/3] Lvl: %079level% Exp: %079experience% Energy: %079energy%</color>" },
            { RoleTypeId.Scp096, "<color=#D51D1D>SCP-096 [%healthpercent%%] %distance%</color>" },
            { RoleTypeId.Scp173, "<color=#D51D1D>SCP-173 [%healthpercent%%] %distance%</color>" },
            { RoleTypeId.Scp939, "<color=#D51D1D>SCP-939 [%healthpercent%%] %distance%</color>" },
        };
        [Description("Custom roles integrations. Format: TemporaryData that marks that the player belongs to that role, display string.")]
        public Dictionary<string, string> CustomRolesIntegrations { get; set; } = new Dictionary<string, string>();
        [Description("Display string alignment. Possible values: left, center, right.")]
        public string TextAlignment { get; set; } = "right";
        [Description("Display text position offset.")]
        public sbyte TextPositionOffset { get; set; } = 30;
        [Description("The player seeing the list will be highlighted with the special marker to the left")]
        public bool MarkPlayerInList { get; set; } = false;
        [Description("Player's marker")]
        public string PlayersMarker { get; set; } = "<color=#D51D1D>You --></color>";
    }
}