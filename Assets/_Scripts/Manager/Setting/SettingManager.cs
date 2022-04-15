using Newtonsoft.Json;
using System.Collections;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using ARKit.Data.Setting;
using ARKit.Util;

namespace ARKit.Manager.Setting
{
    public class SettingManager : MonoBehaviour
    {
        #region Singleton
        private static SettingManager instance;
        public static SettingManager Instance { get { return instance; } }

        public static IEnumerator InstanceCR(InitializerManager manager)
        {
            if (instance == null)
            {
                GameObject go = new GameObject("SettingManager");

                instance = go.AddComponent<SettingManager>();
            }

            yield return instance.InitializeCR(manager);
        }
        #endregion

        private SettingData dataDefault;
        public SettingData DataDefault { get { return dataDefault; } }

        private SettingData dataCurrent;
        public SettingData DataCurrent { get { return dataCurrent; } }

        private string path;

        private IEnumerator InitializeCR(InitializerManager manager)
        {
            transform.SetParent(manager.transform);

            yield return SetPropertiesCR();
        }

        private IEnumerator SetPropertiesCR()
        {
            path = $"{Application.persistentDataPath}/setting.json";

            ResourceRequest request = Resources.LoadAsync<TextAsset>("Manager/Setting/setting");

            yield return request;

            string settings = ((TextAsset)request.asset).text;

            bool complete = false;

            new Thread(() =>
            {
                dataDefault = JsonConvert.DeserializeObject<SettingData>(settings);

                try
                {
                    if (File.Exists(path))
                        dataCurrent = JsonConvert.DeserializeObject<SettingData>(File.ReadAllText(path));
                }
                catch
                {

                }

                complete = true;
            }).Start();

            yield return new WaitUntil(() => complete);

            if (dataCurrent != null)
                yield break;

            dataCurrent = JsonConvert.DeserializeObject<SettingData>(settings);

            new Thread(() => File.WriteAllText(path, settings)).Start();
        }

        public async void SaveAction()
        {
            bool complete = false;

            new Thread(() =>
            {
                File.WriteAllText(path, JsonConvert.SerializeObject(dataCurrent, Formatting.Indented));

                complete = true;
            }).Start();

            while (!complete)
                await Task.Yield();

            EventUtil.Setting.SaveComplete?.Invoke();
        }
    }
}
