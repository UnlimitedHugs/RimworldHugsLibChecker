## HugsLib version checker
This is a tiny library to be included with mods that use HugsLib.

It checks that HugsLib is installed and enabled, and its version is at least that required by the mod. If these checks fail, a dialog is displayed at startup, explaining to the player what they can do to fix the situation. The dialog will contain a button to open the browser on the HugsLib release page, or the Steam workshop page if used on the Steam version.

Including this assembly in the mod is entirely optional, but a good idea, since all the player would see otherwise is an unintelligible error on startup if the library is not properly loaded.

The companion library uses an (optional) configuration file: ModName/About/Version.xml. See Mods/HugsLibCompanion/About/Version.xml for a documented example.

The dollar sign at the start of the assembly name is required for proper assembly load order, since multiple assemblies in one mod are loaded in alphabetical order.