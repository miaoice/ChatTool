using AmongUs.GameOptions;
using HarmonyLib;
using Hazel;
using InnerNet;
using System.Linq;
using UnityEngine;
using UnityEngine.ProBuilder;

namespace ChatTool
{
    
    [HarmonyPatch(typeof(ControllerManager), nameof(ControllerManager.Update))]
    class ControllerManagerUpdatePatch
    {
        public static void Postfix()
        {
            // 强制显示聊天框
            if (GetKeysDown(new[] { KeyCode.Return, KeyCode.C, KeyCode.LeftShift}) && !GameStates.IsInGame)
            {
                HudManager.Instance.Chat.SetVisible(true);
                SIGP.SIGD("Shou Chat Button");
                Log.Warning("show chat button");
            }
            //-------------------------
            //房主
            if (!AmongUsClient.Instance.AmHost) return;

            // 关闭会议
            if (GetKeysDown(new[] { KeyCode.Return, KeyCode.M, KeyCode.LeftShift }) && MeetingHud.Instance)
            {
                MeetingHud.Instance.RpcClose();
                Log.Warning("Closed meeting");
            }
            
            // 重置开始时间
            if (Input.GetKeyDown(KeyCode.C) && GameStartManager.InstanceExists && GameStartManager.Instance.startState == GameStartManager.StartingStates.Countdown)
            {
                GameStartManager.Instance.ResetStartState();
                Log.Warning("Reset start time");
            }
            
            // 开始游戏
            if (Input.GetKeyDown(KeyCode.LeftShift) && GameStartManager.InstanceExists && GameStartManager.Instance.startState == GameStartManager.StartingStates.Countdown)
            {
                GameStartManager.Instance.countDownTimer = 0f;
                Log.Warning("Change start time to 0");
            }


            //开场动画测试
            if (Input.GetKeyDown(KeyCode.G) && GameStates.IsFreePlay)
            {
                HudManager.Instance.StartCoroutine(HudManager.Instance.CoFadeFullScreen(Color.clear, Color.black));
                HudManager.Instance.StartCoroutine(DestroyableSingleton<HudManager>.Instance.CoShowIntro());
            }
        }
        
        static bool GetKeysDown(KeyCode[] keys)
        {
            if (keys.Any(k => Input.GetKeyDown(k)) && keys.All(k => Input.GetKey(k)))
            {
                return true;
            }
            return false;
        }
        public static void murderPlayer(PlayerControl target, MurderResultFlags result)
        {
        if (GameStates.IsFreePlay){

            PlayerControl.LocalPlayer.MurderPlayer(target, MurderResultFlags.Succeeded);
            return;
        
        }

        foreach (var item in PlayerControl.AllPlayerControls)
        {
            MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)RpcCalls.MurderPlayer, SendOption.None, AmongUsClient.Instance.GetClientIdFromCharacter(item));
            writer.WriteNetObject(target);
            writer.Write((int)result);
            AmongUsClient.Instance.FinishRpcImmediately(writer);
        }
    }
        
    }
    
}
