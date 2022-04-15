#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;

namespace KitAR.Tool
{
    public class ClearTool
    {
        [MenuItem("KitAR/Clear/Clear Player Prefs")]
        public static void ClearPlayerPrefs()
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
        }

        [MenuItem("KitAR/Clear/Clear Persistence")]
        public static void ClearPersistence()
        {
            string[] files = Directory.GetFiles(Application.persistentDataPath);

            foreach (string file in files)
                File.Delete(file);
        }
    }
}
#endif