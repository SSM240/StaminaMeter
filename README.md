# Stamina Meter
A mod for Celeste that adds a stamina meter display, partially based off an unused stamina display found in the game's code.

This repo is just for the code - see [the GameBanana page](https://gamebanana.com/mods/34280) for more info and releases.

### Note for building

Unlike most Celeste mods, the csproj file references the XNA version of the game rather than the FNA version, 
so you may need to change the references locally if you're using the FNA version.
Removing all the `Microsoft.Xna.Framework` DLLs and referencing `FNA.dll` instead should suffice.
