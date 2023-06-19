using System;
using UnityEngine;

namespace DevMinecraftMod.Scripts.Utils
{
    public class MinecraftLogger : MonoBehaviour
    {
        public static void Log(string message)
        {
            Debug.Log($"[DevMinecraftMod] {message} [{DateTime.Now}]");
        }

        public static void LogWarning(string message)
        {
            Debug.LogWarning($"[DevMinecraftMod] {message} [{DateTime.Now}]");
        }

        public static void LogError(string message)
        {
            Debug.LogError($"[DevMinecraftMod] {message} [{DateTime.Now}]");
        }
    }
}
