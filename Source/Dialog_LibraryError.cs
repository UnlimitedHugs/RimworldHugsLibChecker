using UnityEngine;
using Verse;
using Verse.Steam;

namespace HugsLibChecker {
	public class Dialog_LibraryError : Window {
		private const string DownloadButtonCaption = "Download now";
		private readonly Color DownloadButtonColor = Color.green;
		private readonly Vector2 DownloadButtonSize = new Vector2(180, 40);
		private const string GitHubDownloadUrl = "https://github.com/UnlimitedHugs/RimworldHugsLib/releases/latest";
		private const string SteamDownloadUrl = "http://steamcommunity.com/sharedfiles/filedetails/?id=818773962";

		private readonly string title;
		private readonly string message;
		private readonly bool showDownloadButton;
		private bool closedLogWindow;

		public override Vector2 InitialSize {
			get { return new Vector2(500f, 400f); }
		}
		
		public Dialog_LibraryError(string title, string message, bool showDownloadButton) {
			this.title = title;
			this.message = message;
			this.showDownloadButton = showDownloadButton;
			closeOnCancel = true;
			doCloseButton = false;
			doCloseX = false;
			forcePause = true;
			absorbInputAroundWindow = true;
		}

		public override void PostClose() {
			base.PostClose();
			if (closedLogWindow) {
				EditWindow_Log.wantsToOpen = true;
			}
		}

		public override void DoWindowContents(Rect inRect) {
			var logWindow = Find.WindowStack.WindowOfType<EditWindow_Log>();
			if (logWindow != null) {
				// hide the log window while we are open
				logWindow.Close(false);
				closedLogWindow = true;
			}
			Text.Font = GameFont.Medium;
			var titleRect = new Rect(inRect.x, inRect.y, inRect.width, 40);
			Widgets.Label(titleRect, title);
			Text.Font = GameFont.Small;
			Widgets.Label(new Rect(inRect.x, inRect.y + titleRect.height, inRect.width, inRect.height - DownloadButtonSize.y - titleRect.height), message);
			Rect closeButtonRect;
			if (showDownloadButton) {
				var prevColor = GUI.color;
				GUI.color = DownloadButtonColor;
				var downloadButtonRect = new Rect(inRect.x, inRect.height - DownloadButtonSize.y, DownloadButtonSize.x, DownloadButtonSize.y);
				if (Widgets.ButtonText(downloadButtonRect, DownloadButtonCaption)) {
					Close();
					var url = SteamManager.Initialized ? SteamDownloadUrl : GitHubDownloadUrl;
					Application.OpenURL(url);
				}
				GUI.color = prevColor;
				closeButtonRect = new Rect(inRect.width - CloseButSize.x, inRect.height - CloseButSize.y, CloseButSize.x, CloseButSize.y);
			} else {	
				closeButtonRect = new Rect(inRect.width/2f - CloseButSize.x/2f, inRect.height - CloseButSize.y, CloseButSize.x, CloseButSize.y);
			}
			if (Widgets.ButtonText(closeButtonRect, "CloseButton".Translate())) {
				Close();
			}
		}
	}
}