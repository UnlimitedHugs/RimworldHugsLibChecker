using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Verse;

namespace HugsLibChecker {
	// do not move or rename. Has detection by full class name
	public class HugsLibChecker : Mod {
		private const string TokenObjectName = "HugsLibCheckerToken";
		private const string LibraryModName = "HugsLib";
		private const string MissingLibraryTitle = "Missing library";
		private const string MissingLibraryMessage = "<b>{0}</b> requires the <b>HugsLib library</b> to work properly.\nWould you like to download it now?";
		private const string OutdatedLibraryTitle = "Library update required";
		private const string OutdatedLibraryMessage = "<b>{0}</b> requires version <b>{1}</b> of the <b>HugsLib library</b> to work properly.\nWould you like to update it now?";
		private const string ImproperLoadOrderTitle = "Improper mod load order";
		private const string ImproperLoadOrderMessage = "The <b>HugsLib library</b> must appear before the mods that use it in the mod load order to work properly.\nPlease rearrange your mods and restart your game.";

		// do not rename- referenced by reflection
		public static bool ChecksPerformed;

		// entry point
		public HugsLibChecker(ModContentPack content) : base(content) {
			RunAllChecks();
		}

		private void RunAllChecks() {
			try {
				if (ChecksAlreadyPerformed()) {
					return;
				}
				var relatedMods = EnumerateLibraryRelatedMods();
				if (!CheckerHasHighestAvailableVersion(relatedMods)) {
					return;
				}
				if (!LibraryIsLoaded(relatedMods)) {
					ScheduleDialog(MissingLibraryTitle, String.Format(MissingLibraryMessage, GetFirstLibraryConsumerName(relatedMods)), true);
				} else if (!LibraryIsUpToDate(relatedMods)) {
					string consumerName;
					var requiredVersion = GetHighestRequiredLibraryVersion(relatedMods, out consumerName);
					ScheduleDialog(OutdatedLibraryTitle, String.Format(OutdatedLibraryMessage, consumerName, requiredVersion), true);
				} else if (!LibraryIsLoadedBeforeConsumers(relatedMods)) {
					ScheduleDialog(ImproperLoadOrderTitle, ImproperLoadOrderMessage, false);
				}
			} catch (Exception e) {
				Log.Error("An exception was caused by the HugsLibChecker assembly. Exception was: "+e);
			}
		}

		private bool CheckerHasHighestAvailableVersion(List<LibraryRelatedMod> relatedMods) {
			var maxVersion = relatedMods.SelectMany(m => m.checkerAssemblies)
				.Select(a => a.GetName().Version)
				.Aggregate((vmax, v) => v > vmax ? v : vmax);
			return GetType().Assembly.GetName().Version == maxVersion;
		}

		private bool LibraryIsLoadedBeforeConsumers(List<LibraryRelatedMod> relatedMods) {
			return relatedMods.Count > 0 && relatedMods[0].isLibrary;
		}

		private bool ChecksAlreadyPerformed() {
			var checkerTypeName = typeof(HugsLibChecker).FullName;
			var checkerTypes = AppDomain.CurrentDomain.GetAssemblies()
				.Select(a => a == null ? null : a.GetType(checkerTypeName, false)).Where(t => t != null);
			var anyAssemblyAlreadyPerformedChecks = checkerTypes.Select(t => t.GetField("ChecksPerformed", BindingFlags.Public | BindingFlags.Static))
				.Any(f => f != null && (bool)f.GetValue(null));
			if (anyAssemblyAlreadyPerformedChecks) {
				return true;
			}
			ChecksPerformed = true;
			return false;
		}

		private List<LibraryRelatedMod> EnumerateLibraryRelatedMods() {
			var checkerTypeName = typeof(HugsLibChecker).FullName;
			var relatedMods = new List<LibraryRelatedMod>();
			foreach (var modContentPack in LoadedModManager.RunningMods) {
				var versionFile = VersionFile.TryParseVersionFile(modContentPack);
				var checkerAssemblies = modContentPack.assemblies.loadedAssemblies.Where(a => a.GetType(checkerTypeName, false) != null).ToList();
				if (checkerAssemblies.Count > 0 || versionFile != null) {
					relatedMods.Add(new LibraryRelatedMod(modContentPack.Name, versionFile, checkerAssemblies));
				}
			}
			return relatedMods;
		}

		private bool LibraryIsLoaded(List<LibraryRelatedMod> relatedMods) {
			for (int i = 0; i < relatedMods.Count; i++) {
				var mod = relatedMods[i];
				if (mod.isLibrary) return true;
			}
			return false;
		}

		private bool LibraryIsUpToDate(List<LibraryRelatedMod> relatedMods) {
			string consumerName;
			var libraryVersion = TryGetLibraryVersion(relatedMods);
			var requiredVersion = GetHighestRequiredLibraryVersion(relatedMods, out consumerName);
			return libraryVersion != null && libraryVersion >= requiredVersion;
		}

		private string GetFirstLibraryConsumerName(List<LibraryRelatedMod> relatedMods) {
			for (int i = 0; i < relatedMods.Count; i++) {
				if (!relatedMods[i].isLibrary) return relatedMods[i].name;
			}
			return "";
		}

		private Version GetHighestRequiredLibraryVersion(List<LibraryRelatedMod> relatedMods, out string consumerName) {
			var maxRequiredVersion = new Version();
			consumerName = "";
			for (int i = 0; i < relatedMods.Count; i++) {
				var mod = relatedMods[i];
				if (mod.isLibrary) continue;
				if (mod.file == null || mod.file.RequiredLibraryVersion == null) continue;
				if (maxRequiredVersion < mod.file.RequiredLibraryVersion) {
					consumerName = mod.name;
					maxRequiredVersion = mod.file.RequiredLibraryVersion;
				}
			}
			return maxRequiredVersion;
		}

		private Version TryGetLibraryVersion(List<LibraryRelatedMod> relatedMods) {
			Version libraryVersion = null;
			for (int i = 0; i < relatedMods.Count; i++) {
				var mod = relatedMods[i];
				if (!mod.isLibrary) continue;
				if (mod.file == null) throw new Exception("Library is missing Version file");
				libraryVersion = mod.file.OverrideVersion;
			}
			return libraryVersion;
		}

		private void ScheduleDialog(string title, string message, bool showDownloadButton) {
			LongEventHandler.QueueLongEvent(() => {
				Find.WindowStack.Add(new Dialog_LibraryError(title, message, showDownloadButton));
			}, null, false, null);
		}
		
		private class LibraryRelatedMod {
			public readonly string name;
			public readonly VersionFile file;
			public readonly bool isLibrary;
			public readonly List<Assembly> checkerAssemblies; 

			public LibraryRelatedMod(string name, VersionFile file, List<Assembly> checkerAssemblies) {
				this.name = name;
				this.file = file;
				this.checkerAssemblies = checkerAssemblies;
				isLibrary = name == LibraryModName;
			}
		}
	}
}