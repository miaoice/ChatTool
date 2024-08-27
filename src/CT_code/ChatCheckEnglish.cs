using System;
using System.IO;
using System.Reflection;
using HarmonyLib;
using UnityEngine;

namespace ChatTool
{
    [HarmonyPatch(typeof(ChatBubble))]
    public static class ChatCE
    {
        public static string ColorString(Color32 color, string str) => $"<size=75%>{str}</size>";
        private static string[] Warning_words;

        static ChatCE()
        {
            LoadWarning_wordsFromFile("ChatTool.Resources.Warningwords_EN.txt");
        }

        

        [HarmonyPatch(nameof(ChatBubble.SetText)), HarmonyPrefix]
        public static void SetText_Prefix(ChatBubble __instance, ref string chatText)
        {
            var sr = __instance.transform.Find("Background").GetComponent<SpriteRenderer>();
            sr.color = new Color(0, 0, 0, 0);

            foreach (string word in Warning_words)
            {
                if (chatText.Contains(word))
                {
                    SIGP.ACW("<align=center><size=125%>Warning Message</size></align>");
                    return; // Only process the first occurrence of a sensitive word
                }
            }

            chatText = ColorString(Color.white, chatText.TrimEnd('\0'));
        }

        private static void LoadWarning_wordsFromFile(string filename)
        {
            try
            {
                // Get the current assembly
                Assembly assembly = Assembly.GetExecutingAssembly();

                // Load the embedded resource file content
                using (Stream stream = assembly.GetManifestResourceStream(filename))
                {
                    if (stream != null)
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            // Read all lines of the file and remove empty lines
                            Warning_words = reader.ReadToEnd().Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                        }
                    }
                    else
                    {
                        Debug.LogError($"Can't load {filename}");
                        Warning_words = new string[0]; // Assign an empty array in case of failure
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error loading sensitive words file from embedded resources: {ex.Message}");
                Warning_words = new string[0]; // Handle exceptions by assigning an empty array
            }
        }
    }
}
