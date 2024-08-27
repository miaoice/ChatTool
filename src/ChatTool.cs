using BepInEx;
using BepInEx.Unity.IL2CPP;
using UnityEngine.SceneManagement;
using System;
using System.Collections.Generic;
using BepInEx.Configuration;
using HarmonyLib;


namespace ChatTool;

[BepInAutoPlugin]
[BepInProcess("Among Us.exe")]
public partial class ChatTool : BasePlugin
{

    public const string PluginGuid = "Not.mb.ChatTool";
    public Harmony Harmony { get; } = new Harmony(PluginGuid);
    public static ConfigEntry<string> menuKeybind;
    

    public override void Load()
    {
        Harmony.PatchAll();
        SceneManager.add_sceneLoaded((Action<Scene, LoadSceneMode>) ((scene, _) =>
        {
            if (scene.name == "MainMenu")
            {
                ModManager.Instance.ShowModStamp(); 
            }
        }));
    }
 }