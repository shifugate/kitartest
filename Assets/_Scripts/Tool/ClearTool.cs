#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;

namespace ARKit.Tool
{
    public class ClearTool
    {
        [MenuItem("ARKit/Clear/Clear Player Prefs")]
        public static void ClearPlayerPrefs()
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
        }

        [MenuItem("ARKit/Clear/Clear Persistence")]
        public static void ClearPersistence()
        {
            string[] files = Directory.GetFiles(Application.persistentDataPath);

            foreach (string file in files)
                File.Delete(file);
        }
    }
}
#endif