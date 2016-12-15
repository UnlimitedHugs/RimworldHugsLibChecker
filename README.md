## HugsLib version checker

**Notice: this assembly is intended for use with the A16 version of HugsLib.**

This is a tiny library to be included with mods that use HugsLib.

[Download the DLL](https://github.com/UnlimitedHugs/RimworldHugsLibChecker/blob/master/Mods/HugsLibChecker/Assemblies/%24HugsLibChecker.dll)

[Sample configuration file](https://github.com/UnlimitedHugs/RimworldHugsLibChecker/blob/master/Mods/HugsLibChecker/About/Version.xml)

[HugsLib on the Steam workshop](http://steamcommunity.com/sharedfiles/filedetails/?id=818773962)

It checks that HugsLib is installed and enabled, and its version is at least that required by the mod. It also checks that the library is loaded before the mods that use it in the mod load order. If any of these checks fail, a dialog is displayed at startup, explaining to the player what they can do to fix the situation. The dialog will contain a button to open the browser on the HugsLib release page, or the Steam workshop page if used on the Steam version.

Note, that it's a good idea to add HugsLib as a "Required Item" on your Steam workshop page to allow the library to be installed automatically with your mod.

Including this assembly in the mod is entirely optional, but a good idea, since all the player would see otherwise is an unintelligible error on startup if the library is not properly loaded.

The companion library uses an (optional) configuration file: ModName/About/Version.xml. Use the link above to see a documented example.

The dollar sign at the start of the assembly name is required for proper assembly load order, since multiple assemblies in one mod are loaded in alphabetical order.