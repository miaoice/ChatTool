using Epic.OnlineServices.Presence;
using HarmonyLib;
using InnerNet;
using System.Linq;
using UnityEngine;
using UnityEngine.ProBuilder;
namespace ChatTool
{
    
    class C
    {
        public static bool CDS = false;
    }
    
    [HarmonyPatch(typeof(ControllerManager), nameof(ControllerManager.Update))]
    class ControllerManagerUpdatePatch
    {
        public static void Postfix()
        {
            if (Input.GetKeyDown(KeyCode.LeftControl) && GameStates.IsLobby)
                PlayerControl.LocalPlayer.GetComponent<CircleCollider2D>().enabled = !PlayerControl.LocalPlayer.gameObject.GetComponent<CircleCollider2D>().enabled;

            // 强制显示聊天框
            if (GetKeysDown(new[] { KeyCode.Return, KeyCode.C, KeyCode.LeftShift}))
            {
                HudManager.Instance.Chat.SetVisible(true);
                SIGP.SIGD("Shou Chat Button");
                Log.Warning("show chat button");
            }
            //-------------------------
            //作弊模式
            if (GetKeysDown(new[] { KeyCode.F1 }))
            {
                if (!C.CDS)
                {
                    C.CDS = true;
                    SIGP.SIGD("CDS ON");
                }else if (C.CDS)
                {
                    C.CDS = !C.CDS;
                    SIGP.SIGD("CDS OFF");
                }
            }
            //内鬼CD => 0s
            if (Input.GetKeyDown(KeyCode.F2))
            {
                if(C.CDS)
                {
                PlayerControl.LocalPlayer.Data.Object.SetKillTimer(0f);
                SIGP.SIGD("Set kill cooldown to 0");
                }
            }
            //做完任务
            if (Input.GetKeyDown(KeyCode.F3))
            {if(C.CDS)
                {
                foreach (var task in PlayerControl.LocalPlayer.myTasks)
                PlayerControl.LocalPlayer.RpcCompleteTask(task.Id);
                SIGP.SIGD("Complete all tasks");
                }
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

            
            //仅测试
            
            if (GetKeysDown(new[] {KeyCode.K}))
            
            {
                SIGP.SIGD("Only test(press K)");
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
        
    }
}
