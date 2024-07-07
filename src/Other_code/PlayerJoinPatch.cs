using HarmonyLib;
using InnerNet;

namespace ChatTool;

[HarmonyPatch(typeof(AmongUsClient), nameof(AmongUsClient.OnPlayerJoined))]
class OnPlayerJoinedPatch
{
    //private static int CID;
    public static void Postfix(AmongUsClient __instance, [HarmonyArgument(0)] ClientData client)
    {
        SIGP.SIGD(

              TranslationController.Instance.currentLanguage.languageID == SupportedLangs.English 
            ? $"<color=#EE9D26>{client.PlayerName} <color=#1F36A2>Join the Room</color>\n<size=80%>({client.FriendCode})</size>"
            : TranslationController.Instance.currentLanguage.languageID == SupportedLangs.SChinese
              ? $"<color=#EE9D26>{client.PlayerName} <color=#1F36A2>加入房间</color>\n<size=80%>({client.FriendCode})</size>"
            : TranslationController.Instance.currentLanguage.languageID == SupportedLangs.TChinese
              ? $"<color=#EE9D26>{client.PlayerName} <color=#1F36A2>加入房間</color>\n<size=80%>({client.FriendCode})</size>"
            : TranslationController.Instance.currentLanguage.languageID == SupportedLangs.Japanese
              ? $"<color=#EE9D26>{client.PlayerName} <color=#1F36A2>部屋に参加</color>\n<size=80%>({client.FriendCode})</size>"
            : TranslationController.Instance.currentLanguage.languageID == SupportedLangs.Korean
              ? $"<color=#EE9D26>{client.PlayerName} <color=#1F36A2>방에 참가</color>\n<size=80%>({client.FriendCode})</size>"
            : TranslationController.Instance.currentLanguage.languageID == SupportedLangs.Russian
              ? $"<color=#EE9D26>{client.PlayerName} <color=#1F36A2>Вступил в комнату</color>\n<size=80%>({client.FriendCode})</size>"
            : TranslationController.Instance.currentLanguage.languageID == SupportedLangs.French
              ? $"<color=#EE9D26>{client.PlayerName} <color=#1F36A2>Rejoindre la salle</color>\n<size=80%>({client.FriendCode})</size>"
            : TranslationController.Instance.currentLanguage.languageID == SupportedLangs.Spanish
              ? $"<color=#EE9D26>{client.PlayerName} <color=#1F36A2>¡Se unió a la sala!</color>\n<size=80%>({client.FriendCode})</size>"
            : TranslationController.Instance.currentLanguage.languageID == SupportedLangs.German
              ? $"<color=#EE9D26>{client.PlayerName} <color=#1F36A2>Zur Räume gegangen</color>\n<size=80%>({client.FriendCode})</size>"
            : TranslationController.Instance.currentLanguage.languageID == SupportedLangs.Portuguese
              ? $"<color=#EE9D26>{client.PlayerName} <color=#1F36A2>Entrou na sala</color>\n<size=80%>({client.FriendCode})</size>"
            : TranslationController.Instance.currentLanguage.languageID == SupportedLangs.Italian
              ? $"<color=#EE9D26>{client.PlayerName} <color=#1F36A2>Ha entrato nella stanza</color>\n<size=80%>({client.FriendCode})</size>"
            : TranslationController.Instance.currentLanguage.languageID == SupportedLangs.Latam
              ? $"<color=#EE9D26>{client.PlayerName} <color=#1F36A2>¡Se unió a la sala!</color>\n<size=80%>({client.FriendCode})</size>"
            : TranslationController.Instance.currentLanguage.languageID == SupportedLangs.Brazilian
              ? $"<color=#EE9D26>{client.PlayerName} <color=#1F36A2>Entrou na sala</color>\n<size=80%>({client.FriendCode})</size>"
            : TranslationController.Instance.currentLanguage.languageID == SupportedLangs.Dutch
              ? $"<color=#EE9D26>{client.PlayerName} <color=#1F36A2>Deel is gekomen</color>\n<size=80%>({client.FriendCode})</size>"
            : TranslationController.Instance.currentLanguage.languageID == SupportedLangs.Filipino
              ? $"<color=#EE9D26>{client.PlayerName} <color=#1F36A2>Nag-pumunta sa sala</color>\n<size=80%>({client.FriendCode})</size>"
            : TranslationController.Instance.currentLanguage.languageID == SupportedLangs.Irish
              ? $"<color=#EE9D26>{client.PlayerName} <color=#1F36A2>Rangadh an tòc</color>\n<size=80%>({client.FriendCode})</size>"
            : $"<color=#EE9D26>{client.PlayerName} <color=#1F36A2>Join the Room</color>\n<size=80%>({client.FriendCode})</size>"
            
    );
    
    }
}
