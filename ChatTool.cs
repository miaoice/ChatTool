using BepInEx;
using BepInEx.Unity.IL2CPP;
using UnityEngine.SceneManagement;
using System;
using UnityEngine;
using UnityEngine.Analytics;
using System.Collections.Generic;
using BepInEx.Configuration;
using HarmonyLib;

namespace ChatTool;

[BepInAutoPlugin]
[BepInProcess("Among Us.exe")]
public partial class ChatTool : BasePlugin
{
    public Harmony Harmony { get; } = new(Id);
    public static string malumVersion = "2.4.0";
    public static List<string> supportedAU = new List<string> { "2024.6.18" };
    public static ConfigEntry<bool> noTelemetry;

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
    


