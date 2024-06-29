using System.Collections.Generic;
using HarmonyLib;

namespace ChatTool
{
    [HarmonyPatch(typeof(ChatController), nameof(ChatController.SendChat))]
    class ChathistoryPatch
    {
        public static List<string> ChatHistory = new();

        public static bool Prefix(ChatController __instance)
        {
            // クイックチャットなら横流し
            if (__instance.quickChatField.Visible)
            {
                return true;
            }
            // 入力欄に何も書かれてなければブロック
            if (__instance.freeChatField.textArea.text == "")
            {
                return false;
            }
            __instance.timeSinceLastMessage = 3f;
            var text = __instance.freeChatField.textArea.text;
            if (ChatHistory.Count == 0 || ChatHistory[^1] != text) ChatHistory.Add(text);
            ChatControllerUpdatePatch.CurrentHistorySelection = ChatHistory.Count;
            string[] args = text.Split(' ');
            var canceled = false;
            
            if (canceled)
            {}
            return !canceled;
        }

        
            }
    }
    
    