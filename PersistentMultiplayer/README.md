### 🚧 Work In Progress 🚧
_This mod is currently under development. Features may be added, removed or changed. Use with caution!_

---

# Persistent Multiplayer (Dedicated & Local)

A Stardew Valley mod which allows you to host a 24/7 server with support for both local and dedicated 
server environments. 

**Key features include:**
 - Pauses the game when no players are actively playing (including host character).
 - Automates the host character as required when not being played and farmhands are connected.
 - Prevents progression when the host character is away/inactive by having the host character
   - Allow events and festivals to happen
   - Checking mail every day
   - Interacting with cutscenes and dialogue boxes
   - Making the appropriate dialogue box choices based on mod configuration
 - Base server commands and configuration options

**Potential Future Features:**
 - Extend server commands (assign admins, kick, ban, mute etc)
 - Plugin support (allow devs to release custom commands / integrations, i.e. `!jail` plugin)
 - Notification system (announce important happenings, i.e. cave mushrooms unlocked)
 - Host character automation includes jobs such as watering crops and feeding animals
 - Host character no longer sleeps in main farmhouse bed, freeing it up for farmhands

#### Dedicated Server Mode

Use this mode when you are hosting your server remotely or from within a container (i.e. when playing the host 
character is difficult to do because of VNC, RDP or similar), the mod will attempt to remove the nuisance of having a host 
character being mostly unplayable, blocking progression and taking up space in your farm.

If you have access to your server's instance of Stardew Valley, you may still control the host character by turning 
ServerMode off (pressing the appropriate keybind, default: `F10`).

### Local Server Mode

Use this mode when you are hosting the server from the same instance of the game you intend to play from, the mod
will not be intrusive when you are actively playing as the host character.

Enable server mode (default: `F10`) when you wish to set yourself as away and your farmhands may still connect, play 
and leave the game as they wish, the game will pause when no farmhands are connected.

ℹ️  _If you are unsure which server mode to use, you should most likely use Local Server mode._

## Acknowledgements

Inspired by [Always On Server for Multiplayer](https://github.com/funny-snek/Always-On-Server-for-Multiplayer) by 
[funny-snek](https://github.com/funny-snek/Always-On-Server-for-Multiplayer) which provided my friends and I 
many hours  of entertainment.

Acknowledgement to [norimicry](https://github.com/norimicry) for his fork of 
[stardew-multiplayer-docker](https://github.com/norimicry/stardew-multiplayer-docker/) which I used as a basis to host 
my own 24/7 dedicated server and for the testing of this mod during development.