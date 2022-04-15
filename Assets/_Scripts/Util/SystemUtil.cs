using System;
using UnityEngine;

namespace KitAR.Util
{
    public static class SystemUtil
    {
        public enum LogType { None, Warning, Exception };

        public static void QuitApplication()
        {
#if !UNITY_EDITOR
            Application.Quit();
#else
            UnityEditor.EditorApplication.isPlaying = false; 
#endif
        }

        public static string UniqueToken()
        {
            return Guid.NewGuid().ToString().Replace("-", "");
        }

        public static void Log(Exception ex, LogType mode = LogType.None)
        {
            Log(Type.Missing.GetType(), ex.ToString(), mode);
        }

        public static void Log(String ex, LogType mode = LogType.None)
        {
            Log(Type.Missing.GetType(), ex, mode);
        }

        public static void Log(Type type, Exception ex, LogType mode = LogType.None)
        {
            Log(type, ex.ToString(), mode);
        }

        public static void Log(Type type, String ex, LogType mode = LogType.None)
        {
            if (mode == LogType.Warning)
                Debug.LogWarning($"{type.Name}: {ex}");
            else if (mode == LogType.Exception)
                Debug.LogError($"{type.Name}: {ex}");
            else
                Debug.Log($"{type.Name}: {ex}");
        }
    }
}
