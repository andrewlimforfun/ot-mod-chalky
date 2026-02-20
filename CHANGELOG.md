# Changelog

All notable changes to this project will be documented in this file.

## [0.0.3] - 2026-02-20

### Changed
- Rename command
  - `/chalkyusefeature` (`/fuf`) to  `/chalkytoggle` (`/ft`)

## [0.0.2] - 2026-02-18

### Changed
- Updated manifest description
- Updated short commands
  - `/chalkyusefeature` (`/fuf`) — toggle all features on/off
  - `/chalkyshowcommand` (`/fsc`) — toggle command visibility in chat  

## [0.0.1] - 2026-02-18

### Added
- Chat logging to file with timestamps and channel labels (`[Global]` / `[Local]`)
- Configurable chat log file path via config and `/chalkysetchatlogpath` command
- Option to strip TMP formatting tags from the chat log via `CleanChatLogTags` config and `/chalkycleanchatlogtags` command
- Configurable global and local chat window message limits via `GlobalMessageLimitCount` / `LocalMessageLimitCount` config and `/chalkysetmessagelimit` command
- Raised notification badge cap from hardcoded 99 to configurable value (default 300, max 999) via `NotificationLimit` config
- In-game command system with the following commands:
  - `/chalkyhelp` (`/fh`) — list all commands
  - `/chalkyusefeature` (`/uff`) — toggle all features on/off
  - `/chalkyshowcommand` (`/sfc`) — toggle command visibility in chat
  - `/chalkyusechatlog` (`/fucl`) — toggle chat file logging
  - `/chalkygetchatlogpath` (`/fgclp`) — print current log file path
  - `/chalkysetchatlogpath` (`/fsclp`) — set log file path
  - `/chalkycleanchatlogtags` (`/fcclt`) — toggle TMP tag stripping in log
  - `/chalkysetmessagelimit` (`/fsml`) — set global/local chat window size
