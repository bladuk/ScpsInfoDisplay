# ScpsInfoDisplay (EXILED) ![Downloads](https://img.shields.io/github/downloads/bladuk/ScpsInfoDisplay/total.svg)
When SCP spawns, displays information about players who are in the SCP team. Possible integration with custom roles.

## How to install plugin?
Put ScpsInfoDisplay.dll under the release tab into %appdata%\EXILED\Plugins (Windows) or .config/EXILED/Plugins (Linux) folder.

## Default configs
```yaml
scpsinfodisplay:
# Is the plugin enabled?
  is_enabled: true
  # Display strings. Format: Role, display string.
  display_strings:
    Scp106: '<color=%rolecolor%>SCP-106 [%healthpercent%%] Vigor: %106vigor%% %distance%</color>'
    Scp049: '<color=%rolecolor%>SCP-049 [%healthpercent%% Zombies: %zombies%] %distance%</color>'
    Scp079: '<color=%rolecolor%>SCP-079 [%generators%%engaging%/3] Lvl: %079level% Exp: %079experience% Energy: %079energy%</color>'
    Scp096: <color=%rolecolor%>SCP-096 [%healthpercent%%] %distance%</color>
    Scp173: <color=%rolecolor%>SCP-173 [%healthpercent%%] %distance%</color>
    Scp939: <color=%rolecolor%>SCP-939 [%healthpercent%%] %distance%</color>
  # Custom roles integrations. Format: SessionVariable that marks that the player belongs to that role, display string.
  custom_roles_integrations: {}
  # Display string alignment. Possible values: left, center, right.
  text_alignment: right
  # Display text position offset.
  text_position_offset: 30
  # The player seeing the list will be highlighted with the special marker to the left. Leave it empty if disabled.
  players_marker: <color=%rolecolor%>You --></color>
  # Display debug messages in server console?
  debug: false
```
