using System;
using System.IO;
using System.Reflection;
using HarmonyLib;
using UnityEngine;

namespace ChatTool
{
    [HarmonyPatch(typeof(ChatBubble))]
    public static class ChatBGColorPatch
    {
        public static string ColorString(Color32 color, string str) => $"<size=75%>{str}</size>";
        
        public static void SetText_Prefix(ChatBubble __instance, ref string chatText)
        {
            var sr = __instance.transform.Find("Background").GetComponent<SpriteRenderer>();
            sr.color = new Color(0, 0, 0, 0);

            chatText = ColorString(Color.white, chatText.TrimEnd('\0'));
        }

    }
}
