---

### Deprecation warning

HugsLibChecker is no longer supported, nor required in Rimworld versions 1.1+  
Dependencies between mods can now easily be achieved with [appropriate tags](https://github.com/UnlimitedHugs/RimworldHugsLib/#dependency-tags) in your About.xml file.

---



![Version](https://img.shields.io/badge/Rimworld-1.0-brightgreen.svg)[![NuGet](https://img.shields.io/nuget/v/UnlimitedHugs.Rimworld.HugsLibChecker.svg)](https://www.nuget.org/packages/UnlimitedHugs.Rimworld.HugsLibChecker/)

## HugsLib version checker

[Download the DLL](https://github.com/UnlimitedHugs/RimworldHugsLibChecker/blob/master/Mods/HugsLibChecker/Assemblies/%24HugsLibChecker.dll) (put in /Assemblies folder)

[Sample configuration file](https://github.com/UnlimitedHugs/RimworldHugsLibChecker/blob/master/Mods/HugsLibChecker/About/Version.xml) (put in /About folder)

[HugsLib on the Steam workshop](http://steamcommunity.com/sharedfiles/filedetails/?id=818773962)

It checks that HugsLib is installed and enabled, and its version is at least that required by the mod. It also checks that the library is loaded before the mods that use it in the mod load order. If any of these checks fail, a dialog is displayed at startup, explaining to the player what they can do to fix the situation. The dialog will contain a button to open the browser on the HugsLib release page, or the Steam workshop page if used on the Steam version.

Note, that it's a good idea to add HugsLib as a "Required Item" on your Steam workshop page to allow the library to be installed automatically with your mod.

Including this assembly in the mod is entirely optional, but a good idea, since all the player would see otherwise is an unintelligible error on startup if the library is not properly loaded.

The companion library uses an (optional) configuration file: ModName/About/Version.xml. Use the link above to see a documented example.

The dollar sign at the start of the assembly name is required for proper assembly load order, since multiple assemblies in one mod are loaded in alphabetical order.