using System;
using System.IO;
using System.Reflection;
using HarmonyLib;
using UnityEngine;

namespace ChatTool
{
    [HarmonyPatch(typeof(ChatBubble))]
    public static class ChatBubblePatch
    {
        private static string[] Warning_words;

        static ChatBubblePatch()
        {
            LoadWarning_wordsFromFile("ChatTool.Rescources.Warningwords.txt"); // 
        }

        public static string ColorString(Color32 color, string str) => $"<size=75%>{str}</size>";

        [HarmonyPatch(nameof(ChatBubble.SetText)), HarmonyPrefix]
        public static void SetText_Prefix(ChatBubble __instance, ref string chatText)
        {
            var sr = __instance.transform.Find("Background").GetComponent<SpriteRenderer>();
            sr.color = new Color(0, 0, 0, 0);

            foreach (string word in Warning_words)
            {
                if (chatText.Contains(word))
                {
                    chatText = "<color=#FF0000>[Warning Message]</color>\n" + ColorString(Color.white, chatText.TrimEnd('\0'));
                    return; // 只要发现一个敏感词就处理完毕，无需继续检查
                }
            }

            chatText = ColorString(Color.white, chatText.TrimEnd('\0'));
        }

        private static void LoadWarning_wordsFromFile(string filename)
        {
            try
            {
                // 获取当前程序集
                Assembly assembly = Assembly.GetExecutingAssembly();

                // 从嵌入资源中读取文件内容
                using (Stream stream = assembly.GetManifestResourceStream(filename))
                {
                    if (stream != null)
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            Warning_words = reader.ReadToEnd().Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                        }
                    }
                    else
                    {
                        Debug.LogError($"Failed to load sensitive words file from embedded resources: {filename}");
                        Warning_words = new string[0]; // 加载失败时可以赋予一个默认值
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error loading sensitive words file from embedded resources: {ex.Message}");
                Warning_words = new string[0]; // 处理异常情况，设置一个默认值
            }
        }
    }
}
