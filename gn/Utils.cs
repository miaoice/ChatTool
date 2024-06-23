using UnityEngine;
using InnerNet;
using System.Linq;
using Il2CppSystem.Collections.Generic;
using System.IO;
using Hazel;
using System.Reflection;
using AmongUs.GameOptions;
using Sentry.Internal.Extensions;

namespace ChatTool;
public static class Utils
{
    //Useful for getting full lists of all the Among Us cosmetics IDs
    public static ReferenceDataManager referenceDataManager = DestroyableSingleton<ReferenceDataManager>.Instance;
    public static bool isShip => ShipStatus.Instance != null;
    public static bool isLobby => AmongUsClient.Instance.GameState == InnerNetClient.GameStates.Joined;
    public static bool isOnlineGame => AmongUsClient.Instance.NetworkMode == NetworkModes.OnlineGame;
    public static bool isLocalGame => AmongUsClient.Instance.NetworkMode == NetworkModes.LocalGame;
    public static bool isFreePlay => AmongUsClient.Instance.NetworkMode == NetworkModes.FreePlay;
    public static bool isPlayer => PlayerControl.LocalPlayer != null;
    public static bool isHost = AmongUsClient.Instance.AmHost;
    public static bool isInGame => AmongUsClient.Instance.GameState == InnerNetClient.GameStates.Started && isPlayer;

    //Get ClientData by PlayerControl
    public static ClientData getClientByPlayer(PlayerControl player)
    {
        try
        {
            var client = AmongUsClient.Instance.allClients.ToArray().FirstOrDefault(cd => cd.Character.PlayerId == player.PlayerId);
            return client;
        }
        catch
        {
            return null;
        }
    }

    //Get ClientData.Id by PlayerControl
    public static int getClientIdByPlayer(this PlayerControl player)
    {
        if (player == null) return -1;
        var client = getClientByPlayer(player);
        return client == null ? -1 : client.Id;
    }

    // Adjusts HUD resolution
    // Used to fix UI problems when zooming out
    public static void adjustResolution() {
        ResolutionManager.ResolutionChanged.Invoke((float)Screen.width / Screen.height, Screen.width, Screen.height, Screen.fullScreen);
    }

    // Get RoleBehaviour from a RoleType
    public static RoleBehaviour getBehaviourByRoleType(RoleTypes roleType) {
        return RoleManager.Instance.AllRoles.First(r => r.Role == roleType);
    }

    // Kill any player using RPC calls
    public static void murderPlayer(PlayerControl target, MurderResultFlags result)
    {
        if (isFreePlay){

            PlayerControl.LocalPlayer.RpcMurderPlayer(target, true);
            return;
        
        }

        var HostData = AmongUsClient.Instance.GetHost();
        if (HostData != null && !HostData.Character.Data.Disconnected)
        {
            foreach (var item in PlayerControl.AllPlayerControls)
            {
                MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)RpcCalls.MurderPlayer, SendOption.None, AmongUsClient.Instance.GetClientIdFromCharacter(item));
                writer.WriteNetObject(target);
                writer.Write((int)result);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
            }
        }
    }

    // Report bodies using RPC calls
    public static void reportDeadBody(NetworkedPlayerInfo playerData)
    {

        if (isFreePlay){

            PlayerControl.LocalPlayer.CmdReportDeadBody(playerData);
            return;
        
        }

        var HostData = AmongUsClient.Instance.GetHost();
        if (HostData != null && !HostData.Character.Data.Disconnected)
        {
            MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)RpcCalls.ReportDeadBody, SendOption.None, HostData.Id);
            writer.Write(playerData.PlayerId);
            AmongUsClient.Instance.FinishRpcImmediately(writer);
        }
    }


    // Complete all of LocalPlayer's tasks using RPC calls
    public static void completeMyTasks()
    {

        if (isFreePlay){

            foreach (PlayerTask task in PlayerControl.LocalPlayer.myTasks)
            {
                if (!task.IsComplete){
                    PlayerControl.LocalPlayer.RpcCompleteTask(task.Id);
                }
            }
            return;
        
        }

        var HostData = AmongUsClient.Instance.GetHost();
        if (HostData != null && !HostData.Character.Data.Disconnected)
        {
            foreach (PlayerTask task in PlayerControl.LocalPlayer.myTasks)
            {
                if (!task.IsComplete){

                    foreach (var item in PlayerControl.AllPlayerControls)
                    {
                        MessageWriter messageWriter = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)RpcCalls.CompleteTask, SendOption.None, AmongUsClient.Instance.GetClientIdFromCharacter(item));
                        messageWriter.WritePacked(task.Id);
                        AmongUsClient.Instance.FinishRpcImmediately(messageWriter);
                    }

                }
            }
        }
    }

    // Open Chat UI
    public static void openChat()
    {
        if (!DestroyableSingleton<HudManager>.Instance.Chat.IsOpenOrOpening){
            DestroyableSingleton<HudManager>.Instance.Chat.chatScreen.SetActive(true);
            PlayerControl.LocalPlayer.NetTransform.Halt();
            DestroyableSingleton<HudManager>.Instance.Chat.StartCoroutine(DestroyableSingleton<HudManager>.Instance.Chat.CoOpen());
            if (DestroyableSingleton<FriendsListManager>.InstanceExists)
            {
                DestroyableSingleton<FriendsListManager>.Instance.SetFriendButtonColor(true);
            }
        }

    }

    // Draw a tracer line between two 2 GameObjects
    public static void drawTracer(GameObject sourceObject, GameObject targetObject, Color color)
    {
        LineRenderer lineRenderer;

        lineRenderer = sourceObject.GetComponent<LineRenderer>();

        if(!lineRenderer){
            lineRenderer = sourceObject.AddComponent<LineRenderer>();
        }

        lineRenderer.SetVertexCount(2);
        lineRenderer.SetWidth(0.02F, 0.02F);

        // I just picked an already existing material from the game
        Material material = DestroyableSingleton<HatManager>.Instance.PlayerMaterial;

        lineRenderer.material = material;
        lineRenderer.SetColors(color, color);
                
        lineRenderer.SetPosition(0, sourceObject.transform.position);
        lineRenderer.SetPosition(1, targetObject.transform.position);
    }

    

    

    

    

    // Get SystemType of the room the player is currently in
    public static SystemTypes getCurrentRoom(){
        return HudManager.Instance.roomTracker.LastRoom.RoomId;
    }

    // Fancy colored ping text
    

    // Get a UnityEngine.KeyCode from a string
    public static KeyCode stringToKeycode(string keyCodeStr){

        if(!string.IsNullOrEmpty(keyCodeStr)){ // Empty strings are automatically invalid

            try{
                
                // Case-insensitive parse of UnityEngine.KeyCode to check if string is validssss
                KeyCode keyCode = (KeyCode)System.Enum.Parse(typeof(KeyCode), keyCodeStr, true);
                
                return keyCode;

            }catch{}
        
        }

        return KeyCode.Delete; // If string is invalid, return Delete as the default key
    }

    // Get a platform type from a string
    public static bool stringToPlatformType(string platformStr, out Platforms? platform){

        if(!string.IsNullOrEmpty(platformStr)){ // Empty strings are automatically invalid

            try{
                
                // Case-insensitive parse of Platforms from string (if it valid)
                platform = (Platforms)System.Enum.Parse(typeof(Platforms), platformStr, true);
                
                return true; // If platform type is valid, return false

            }catch{}
        
        }

        platform = null;
        return false; // If platform type is invalid, return false
    }

    // Get the string name for a chosen player's role
    // String are automatically translated
    

    // Show custom popup ingame
    // Found here: https://github.com/NuclearPowered/Reactor/blob/6eb0bf19c30733b78532dada41db068b2b247742/Reactor/Networking/Patches/HttpPatches.cs
    public static void showPopup(string text){
        var popup = Object.Instantiate(DiscordManager.Instance.discordPopup, Camera.main!.transform);
        
        var background = popup.transform.Find("Background").GetComponent<SpriteRenderer>();
        var size = background.size;
        size.x *= 2.5f;
        background.size = size;

        popup.TextAreaTMP.fontSizeMin = 2;
        popup.Show(text);
    }

    // Load sprites and textures from manifest resources
    // Found here: https://github.com/Loonie-Toons/TOHE-Restored/blob/TOHE/Modules/Utils.cs
    public static Dictionary<string, Sprite> CachedSprites = new();
    public static Sprite LoadSprite(string path, float pixelsPerUnit = 1f)
    {
        try
        {
            if (CachedSprites.TryGetValue(path + pixelsPerUnit, out var sprite)) return sprite;

            Texture2D texture = LoadTextureFromResources(path);
            sprite = Sprite.Create(texture, new(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), pixelsPerUnit);
            sprite.hideFlags |= HideFlags.HideAndDontSave | HideFlags.DontSaveInEditor;

            return CachedSprites[path + pixelsPerUnit] = sprite;
        }
        catch
        {
            Debug.LogError($"Failed to read Texture: {path}");
        }
        return null;
    }
    public static Texture2D LoadTextureFromResources(string path)
    {
        try
        {
            var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(path);
            var texture = new Texture2D(1, 1, TextureFormat.ARGB32, false);
            using MemoryStream ms = new();
            
            stream.CopyTo(ms);
            ImageConversion.LoadImage(texture, ms.ToArray(), false);
            return texture;
        }
        catch
        {
            Debug.LogError($"Failed to read Texture: {path}");
        }
        return null;
    }
}

