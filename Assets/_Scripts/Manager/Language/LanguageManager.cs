using KitAR.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using KitAR.Manager.Language.Map;

namespace KitAR.Manager.Language
{
    public class LanguageManagerUpdate : UnityEvent { }

    public class LanguageManager : MonoBehaviour
    {
        #region Singleton
        private static LanguageManager instance;
        public static LanguageManager Instance { get { return instance; } }

        public static void InstanceNW(InitializerManager manager)
        {
            if (instance == null)
            {
                GameObject go = new GameObject("LanguageManager");

                instance = go.AddComponent<LanguageManager>();
            }

            instance.Initialize(manager);
        }
        #endregion

        public static LanguageManagerUpdate languageManagerUpdate = new LanguageManagerUpdate();

        public enum CountryCode { en_US, pt_PT }

        private Dictionary<string, Dictionary<string, Dictionary<string, string>>> contents = new Dictionary<string, Dictionary<string, Dictionary<string, string>>>();

        private string language;
        public string Language { get { return language; } }

        private List<LanguageManagerMap> maps = new List<LanguageManagerMap>();

        private void Initialize(InitializerManager manager)
        {
            transform.SetParent(manager.transform);

            SetProperties();
        }

        private void SetProperties()
        {
            contents.Clear();

            TextAsset[] assets = Resources.LoadAll<TextAsset>("Manager/Language");

            foreach (TextAsset asset in assets)
                contents.Add(asset.name, JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(asset.text));

            language = PlayerPrefs.HasKey("language") ? PlayerPrefs.GetString("language") : Enum.GetName(typeof(CountryCode), CountryCode.pt_PT);

            if (!Enum.TryParse(language, out CountryCode result))
                language = Enum.GetName(typeof(CountryCode), CountryCode.pt_PT);
        }

        public void SetLanguage(CountryCode language)
        {
            this.language = Enum.GetName(typeof(CountryCode), language);

            PlayerPrefs.SetString("language", this.language);
            PlayerPrefs.Save();

            UpdateMaps();

            languageManagerUpdate?.Invoke();
        }

        public string GetTranslation(string group, string key)
        {
            try
            {
                return contents[language][group][key];
            }
            catch (Exception ex)
            {
                SystemUtil.Log(GetType(), $"{language}: {group}: {key} - {ex}", SystemUtil.LogType.Warning);
            }

            return $"{language}: {group}: {key}";
        }

        public void AddLanguageMap(LanguageManagerMap map)
        {
            if (!maps.Contains(map))
                maps.Add(map);

            UpdateMaps(map);
        }

        public void RemoveLanguageMap(LanguageManagerMap map)
        {
            maps.Remove(map);
        }

        private void UpdateMaps(LanguageManagerMap map = null)
        {
            if (map != null)
            {
                SetMapTranslation(map);

                return;
            }

            foreach(LanguageManagerMap obj in maps)
                SetMapTranslation(obj);
        }

        private void SetMapTranslation(LanguageManagerMap map)
        {
            try
            {
                Component[] components = map.GetComponents<Component>().Where(x => x.GetType().GetProperty("text") != null).ToArray();

                foreach (Component component in components)
                {
                    try
                    {
                        component.GetType().GetProperty("text").SetValue(component, map.upper ? GetTranslation(map.group, map.key).ToUpper() : GetTranslation(map.group, map.key));
                    }
                    catch(Exception ex)
                    {
                        SystemUtil.Log(GetType(), $"{language}: {component.gameObject.name}: {map.group}: {map.key} - {ex}", SystemUtil.LogType.Warning);
                    }
                }
            }
            catch (Exception)
            {

            }
        }
    }
}
