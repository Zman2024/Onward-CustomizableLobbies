# Onward 1.7 Customizable Lobbies
A BepInEx mod for Onward 1.7.7 that allows users to create public lobbies with (almost) no restrictions

<h2> What does this do? </h2>
This mod allows you to do the following:

* Modify the properties of a public lobby (round time, timeouts, etc)
* Host public lobbies for gamemodes that were locked to private (Spec Ops, One in The Chamber, Free Roam)
* Change the maximum number of players in your lobby to whatever you want (imagine 10v10 PvP / 20 player PvE)

<h2> How to install </h2>

1. Install BepInEx to Onward (https://github.com/BepInEx/BepInEx/wiki/Installation)

2. Either download a release (https://github.com/Zman2024/Onward-CustomizableLobbies/releases) or build from source

3. Put `CustomizableLobbies.dll` into `Onward/BepInEx/plugins`

4. Boom, installed

<h2> How to change the maximum number of players </h2>

This requires that you start the game at least once with the mod installed, that way it creates the config file in `Onward/BepInEx/config/Zman2024-CustomizableLobbies.cfg`

There are only 2 settings right now: 

* `MaxPlayersPvP` - how many players are allowed in a PvP lobby (default: 10)
* `MaxPlayersCOOP` - how many players are allowed in a PvE lobby (default: 4)

You can change these to whatever you want really, but when i tried to go to 100 things broke

Hopefully more to come from this mod in the future, maybe a way to change the max players at runtime would be nice...
