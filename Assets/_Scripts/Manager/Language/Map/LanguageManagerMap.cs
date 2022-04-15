using UnityEngine;

namespace ARKit.Manager.Language.Map
{
    public class LanguageManagerMap : MonoBehaviour
    {
        [HideInInspector]
        public string groupName;
        [HideInInspector]
        public string group = "common";

        [HideInInspector] 
        public string keyName;
        [HideInInspector] 
        public string key;

        public bool upper;

        private void Awake()
        {
            if (LanguageManager.Instance != null)
                LanguageManager.Instance.AddLanguageMap(this);
        }

        private void OnDestroy()
        {
            if (LanguageManager.Instance != null)
                LanguageManager.Instance.RemoveLanguageMap(this);
        }
    }
}
