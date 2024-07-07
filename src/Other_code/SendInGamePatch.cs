
using Discord;

namespace ChatTool;

public class SIGP
{
    public static void SIGD(string text)
    {
        if (DestroyableSingleton<HudManager>._instance) 
            HudManager.Instance.Notifier.AddDisconnectMessage(text);
    }
    public static void SIGM(string text)
    {
        if (DestroyableSingleton<HudManager>._instance) 
            HudManager.Instance.Notifier.AddDisconnectMessage(text);
    }
}