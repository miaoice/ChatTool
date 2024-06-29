using BepInEx;
using BepInEx.Configuration;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace ChatTool;

[BepInProcess("Among Us.exe")]
public class Main : BasePlugin
{

    public static readonly string ModName = "ChatTool"; // 模组名字
    public const string PluginVersion = "1.1.0"; //咱们模组的版本号
    public static BepInEx.Logging.ManualLogSource Logger;
    
    public static Main Instance; //设置Main实例
    public static ConfigEntry<string> BetaBuildURL { get; private set; }
    public override void Load()//加载 启动！
    {
        Instance = this; //Main实例

        BetaBuildURL = Config.Bind("Other", "BetaBuildURL", "");
        
        Logger = BepInEx.Logging.Logger.CreateLogSource("ChatTool"); //输出前缀

        Logger.LogInfo($"ChatTool loaded.");
    }
}