# Chalky

A BepInEx mod for [On Together](https://store.steampowered.com/app/2688490/On_Together/) that enhances the chalkboard drawing experience with configurable brush sizes and in-game chat commands.

## WARNING

- Chalk enlargement only!
- Chalk size can't be smaller than the default 2 sorry! I use RPC call and that has minimum size 2x2 as determined by the game API.

## Features

- **Configurable chalk brush size** — Paint a larger area on the chalkboard in a single stroke. The default game brush covers a 2×2 grid cell block; Chalky lets you scale this up to any integer size.
- **In-game chat commands** — Control mod behavior on the fly without leaving the game or editing config files. Commands are typed into the chat box and are **never sent** to other players.
- **Toggle on/off at runtime** — Enable or disable the mod's drawing enhancements instantly without restarting the game.
- **Show/hide command echo** — Optionally suppress commands from appearing in your chat history.

## In-Game Commands

Type any command into the in-game chat. Commands start with `/` and are **not sent** to other players. Each command has a short alias for convenience.

- `/chalkyhelp` (`/ch`) — List all available Chalky commands
- `/help chalky` (`/h chalky`) — Same as above, via the general help command
- `/chalkytoggle` (`/ct`) — Toggle the mod on or off
- `/chalkysetsize [size]` (`/css [size]`) — Set the chalk brush size. Omit the argument to reset to default (2)
- `/chalkyshowcommand` (`/csc`) — Toggle whether your commands are echoed in chat

> **Note:** Size changes are not persistent and reset to the default of `2` on game restart.

### Examples

```
# Set brush to paint a 5×5 block per stroke
/css 5

# Reset brush size back to default (2×2)
/css

# Turn off Chalky's drawing enhancements entirely
/ct

# Check which commands are available
/ch
```

## Configuration

Settings are saved to the BepInEx config file and can also be toggled at runtime via chat commands.

**Location:** `BepInEx/config/com.andrewlin.ontogether.chalky.cfg`

- `EnableFeature` (default: `true`) — Enable or disable all mod features
- `ShowCommand` (default: `false`) — Show commands in chat when typed

## Installation

Use `r2modman` for simpler installation.

1. Install [BepInEx](https://github.com/BepInEx/BepInEx/releases) into your On Together game folder
2. Copy `AndrewLin.Chalky.dll` into `BepInEx/plugins/`
3. Launch the game — the mod will load automatically and generate a config file at `BepInEx/config/com.andrewlin.ontogether.chalky.cfg`

## Building from Source

Requires .NET SDK and the game's managed DLLs referenced in the project.

```
dotnet build
```

Output DLL will be in `Chalky/bin/`.
