using System;
using System.Collections.Generic;
using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Unity.IL2CPP;

[assembly: AssemblyFileVersion(ChatTool.Main.PluginVersion)]
[assembly: AssemblyVersion(ChatTool.Main.PluginVersion)]
namespace ChatTool;
[BepInProcess("Among Us.exe")]

public class Main : BasePlugin
{

    public static readonly string ModName = "ChatTool"; // 模组名字
    public const string PluginVersion = "1.2.1"; //版本号
    public static BepInEx.Logging.ManualLogSource Logger;
    
    public static Main Instance; //设置Main实例
    public static ConfigEntry<string> BetaBuildURL { get; private set; }
    public static Version version = Version.Parse(PluginVersion);
    public override void Load()
    {
        Instance = this; 

        BetaBuildURL = Config.Bind("Other", "BetaBuildURL", "");
        
        Logger = BepInEx.Logging.Logger.CreateLogSource("ChatTool"); //输出前缀

        Logger.LogInfo($"ChatTool loaded.");
    }
    
    public static readonly string ForkId = "ChatTool";
    public static Dictionary<byte, PlayerVersion> playerVersion = new();
    
}

public class PlayerVersion
{
    public readonly Version version;

    public readonly string tag;
    
    public readonly string forkId;
    public PlayerVersion(string ver, string tag_str, string forkId) : this(Version.Parse(ver), tag_str, forkId) { }
    public PlayerVersion(Version ver, string tag_str, string forkId)
    {
        version = ver;
        tag = tag_str;
        this.forkId = forkId;
    }
}
