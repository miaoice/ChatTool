
using Discord;
using HarmonyLib;
using Hazel;

namespace ChatTool;

public class SIGP
{
    //退出房间的左下角提示
    public static void SIGD(string text)
    {
        if (DestroyableSingleton<HudManager>._instance) 
            HudManager.Instance.Notifier.AddDisconnectMessage(text);
    }

    //禁止发送网站的那个提示
    public static void ACW(string text)
    {
        if (DestroyableSingleton<HudManager>._instance) 
            HudManager.Instance.Chat.AddChatWarning(text);
    }
}