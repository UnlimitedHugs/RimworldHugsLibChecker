using UnityEngine;
using Verse;

namespace HugsLibChecker {
	public class Dialog_LibraryError : Window {
		private const string DownloadButtonCaption = "Download now";
		private readonly Color DownloadButtonColor = Color.green;
		private readonly Vector2 DownloadButtonSize = new Vector2(180, 40);
		private const string GitHubDownloadUrl = "https://github.com/UnlimitedHugs/RimworldHugsLib/releases/latest";

		private readonly string title;
		private readonly string message;
		private readonly bool showDownloadButton;

		public override Vector2 InitialSize {
			get { return new Vector2(500f, 400f); }
		}
		
		public Dialog_LibraryError(string title, string message, bool showDownloadButton) {
			this.title = title;
			this.message = message;
			this.showDownloadButton = showDownloadButton;
			closeOnEscapeKey = true;
			doCloseButton = false;
			doCloseX = false;
			forcePause = true;
			absorbInputAroundWindow = true;
			layer = WindowLayer.SubSuper;
		}

		public override void DoWindowContents(Rect inRect) {
			Text.Font = GameFont.Medium;
			var titleRect = new Rect(inRect.x, inRect.y, inRect.width, 40);
			Widgets.Label(new Rect(inRect.x, inRect.y, inRect.width, inRect.height - DownloadButtonSize.y), title);
			Text.Font = GameFont.Small;
			Widgets.Label(new Rect(inRect.x, inRect.y + titleRect.height, inRect.width, inRect.height - DownloadButtonSize.y - titleRect.height), message);
			Rect closeButtonRect;
			if (showDownloadButton) {
				var prevColor = GUI.color;
				GUI.color = DownloadButtonColor;
				var downloadButtonRect = new Rect(inRect.x, inRect.height - DownloadButtonSize.y, DownloadButtonSize.x, DownloadButtonSize.y);
				if (Widgets.ButtonText(downloadButtonRect, DownloadButtonCaption)) {
					Close();
					Application.OpenURL(GitHubDownloadUrl);
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