using HarmonyLib;
using UnityEngine;
using TMPro;

namespace ChatTool;

[HarmonyPriority(Priority.Low)]
[HarmonyPatch(typeof(PingTracker), nameof(PingTracker.Update))]
public static class PingTracker_Update
{
    private static float deltaTime;
    
    [HarmonyPostfix]
    public static void Postfix(PingTracker __instance)
    {
        var offset_x = 3.1f; //从右边缘偏移
        var offset_y = 6f; //从右边缘偏移
        if (HudManager.InstanceExists && HudManager._instance.Chat.chatButton.gameObject.active) offset_x -= 0.8f; //如果有聊天按钮，则有额外的偏移量
        __instance.GetComponent<AspectPosition>().DistanceFromEdge = new Vector3(offset_x, offset_y, 0f);

        __instance.text.text = __instance.ToString();
        __instance.text.alignment = TextAlignmentOptions.TopRight;
        __instance.text.text =
            $"<color=#4ABCA4>{Main.ModName}</color><color=#00FFFF> v{Main.PluginVersion}</color>";

        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        float fps = Mathf.Ceil(1.0f / deltaTime);
     __instance.text.text += "\nBy <color=#FFFF00>Miaoice</color>";
        __instance.text.text += Utils.GetPing(AmongUsClient.Instance.Ping) + Utils.GetFps(fps);
        //FPS: <color=#FF0000>" + fps.ToString() + "</color>"
        
    }
}