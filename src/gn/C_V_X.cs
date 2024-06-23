using AmongUs.Data;
using HarmonyLib;
using InnerNet;
using UnityEngine;

namespace ChatTool
{
	[HarmonyPatch(typeof(ChatController), "Update")]
	internal class ChatControllerUpdatePatch
	{
		public static void Prefix()
		{
			bool flag = AmongUsClient.Instance.AmHost && DataManager.Settings.Multiplayer.ChatMode == QuickChatModes.QuickChatOnly;
			if (flag)
			{
				DataManager.Settings.Multiplayer.ChatMode = QuickChatModes.FreeChatOrQuickChat;
			}
		}

		public static void Postfix(ChatController __instance)
		{
			bool flag = !__instance.freeChatField.textArea.hasFocus;
			if (!flag)
			{
				bool flag2 = (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) && Input.GetKeyDown(KeyCode.C);
				if (flag2)
				{
					ClipboardHelper.PutClipboardString(__instance.freeChatField.textArea.text);
				}
				bool flag3 = (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) && Input.GetKeyDown(KeyCode.V);
				if (flag3)
				{
					__instance.freeChatField.textArea.SetText(__instance.freeChatField.textArea.text + GUIUtility.systemCopyBuffer, "");
				}
				bool flag4 = (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) && Input.GetKeyDown(KeyCode.X);
				if (flag4)
				{
					ClipboardHelper.PutClipboardString(__instance.freeChatField.textArea.text);
					__instance.freeChatField.textArea.SetText("", "");
				}
			}
		}

		public static int CurrentHistorySelection = -1;
	}
}
