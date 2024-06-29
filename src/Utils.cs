namespace ChatTool;

public class Utils
{
    public static string GetPing(int ping){

        return $"<color=#00ff00>\nPing: {ping} ms</color>";

        
    }    public static string GetFPS(float fps){

        return $"<color=#1F1F85>\nFPS: {fps}</color>";
        
    }
}