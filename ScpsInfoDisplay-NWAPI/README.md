# ScpsInfoDisplay (NWAPI) ![Downloads](https://img.shields.io/github/downloads/bladuk/ScpsInfoDisplay/total.svg)
When SCP spawns, displays information about players who are in the SCP team. Possible integration with custom roles.

## How to install plugin?
Put ScpsInfoDisplay-NWAPI.dll under the release tab into %appdata%\SCP: Secret Laboratory\PluginAPI\plugins\port (Windows) or .config/SCP: Secret Laboratory/PluginAPI/plugins/port (Linux) folder.

## Default configs
```yaml
scpsinfodisplay:
  # Display strings. Format: Role, display string.
  display_strings:
    Scp106: '<color=#D51D1D>SCP-106 [%healthpercent%%] Vigor: %106vigor%% %distance%</color>'
    Scp049: '<color=#D51D1D>SCP-049 [%healthpercent%% Zombies: %zombies%] %distance%</color>'
    Scp079: '<color=#D51D1D>SCP-079 [%generators%%engaging%/3] Lvl: %079level% Exp: %079experience% Energy: %079energy%</color>'
    Scp096: <color=#D51D1D>SCP-096 [%healthpercent%%] %distance%</color>
    Scp173: <color=#D51D1D>SCP-173 [%healthpercent%%] %distance%</color>
    Scp939: <color=#D51D1D>SCP-939 [%healthpercent%%] %distance%</color>
  # Custom roles integrations. Format: SessionVariable that marks that the player belongs to that role, display string.
  custom_roles_integrations: {}
  # Display string alignment. Possible values: left, center, right.
  text_alignment: right
  # Display text position offset.
  text_position_offset: 30
  # The player seeing the list will be highlighted with the special marker to the left. Leave it empty if disabled.
  players_marker: <color=#D51D1D>You --></color>
```
