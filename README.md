# Chalky

A BepInEx mod for [On Together](https://store.steampowered.com/app/2688490/On_Together/) that enhances the in-game chat with chat logging, configurable message limits, and in-game utility commands.

## Features

- **Chat logging text file** — Saves all chat messages to a file with timestamps and channel labels
- **Configurable chat history length** — Increase how many messages are visible in the global and local chat windows
- **In-game commands** — Toggle and configure all features without leaving the game

## In-Game Commands

Type any command into the in-game chat. Commands start with `/` and are **not sent** to other players. For default values see *Configuration*

- `/chalkyhelp` (`/fh`) — List all available Chalky commands
- `/chalkyusefeature` (`/fuf`) — Toggle all Chalky features on/off
- `/chalkyshowcommand` (`/fsc`) — Toggle whether commands are visible in chat
- `/chalkyusechatlog` (`/fucl`) — Toggle chat file logging on/off
- `/chalkygetchatlogpath` (`/fgclp`) — Print the current chat log file path
- `/chalkysetchatlogpath <path>` (`/fsclp`) — Set a new path for the chat log file
- `/chalkycleanchatlogtags` (`/fcclt`) — Toggle stripping TMP tags from the chat log
- `/chalkysetmessagelimit [global|local] <number>` (`/fsml`) — Set the global or local chat window message limit

### Examples

```
/chalkysetmessagelimit global 100
/fsml local 50
/chalkysetchatlogpath C:\Logs\chat.txt
/chalkyusechatlog
```

## Configuration

Settings can be changed in the config file or using in-game commands (see below).
Located in `{BepInExConfigPath}/com.andrewlin.ontogether.chalky.cfg`

- **General**
  - `EnableFeature` (default: `true`) — Enable or disable all mod features
  - `ShowCommand` (default: `false`) — Show commands in chat when used
  - `EnableChatFileLogging` (default: `true`) — Enable chat log file writing
  - `ChatLogPath` (default: `BepInEx/on_together_chat_log.txt`) — Path to the chat log file
  - `CleanChatLogTags` (default: `false`) — Strip TMP color/formatting tags from the chat log
- **Chat**
  - `GlobalMessageLimitCount` (default: `50`) — Max messages shown in global chat window (game default: 50)
  - `LocalMessageLimitCount` (default: `25`) — Max messages shown in local chat window (game default: 25)

> The notification badge cap (originally hardcoded to 99) is raised to 300 by default and can be changed via `NotificationLimit` in the config.

## Chat Log Format

Each line in the log file looks like:

```
[2026-02-18 21:34:12] [Global] PlayerName: hello world
[2026-02-18 21:34:15] [Local] OtherPlayer: hey!
```

The log file is cleared on each game launch.

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
