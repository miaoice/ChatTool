using System;
using System.Collections.Generic;
using System.Linq;
using AmongUs.GameOptions;
using Sentry.Internal.Extensions;
using UnityEngine;
using static InnerNet.InnerNetClient;

namespace ChatTool;

public class Utils
{    
    public static string GetPing(int ping){

        return $"<color=#9CDCF0>\nPing: {ping} ms</color>";
        
    }  
    public static string GetFps(float fps){

        return $"<color=#9CDCF0>\nFps: {fps}</color>";
        
    }   

    public bool canstartgame = true;
    public static string ColorString(Color32 color, string str) => $"<color=#{color.r:x2}{color.g:x2}{color.b:x2}{color.a:x2}>{str}</color>";

}
