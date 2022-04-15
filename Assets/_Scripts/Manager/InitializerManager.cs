using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using ARKit.Manager.Setting;
using ARKit.Manager.System;
using ARKit.Manager.Language;
using ARKit.Manager.Anchor;

namespace ARKit.Manager
{
    public class InitializerManagerComplete : UnityEvent { }

    public class InitializerManager : MonoBehaviour
    {
        #region Singleton
        private static bool initialized;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void RuntimeInitialize()
        {
            if (initialized)
                return;

            initialized = true;

            Instance.Initialize();
        }

        private static InitializerManager instance;
        public static InitializerManager Instance
        {
            get
            {
                if (instance == null)
                {
                    GameObject go = new GameObject("InitializerManager");

                    instance = go.AddComponent<InitializerManager>();

                    DontDestroyOnLoad(go);
                }

                return instance;
            }
        }
        #endregion

        private static bool initializeComplete;
        public static bool InitializeComplete { get { return initializeComplete; } }
		
        private void Initialize()
        {
            StartCoroutine(InitializeCR());
        }

        private IEnumerator InitializeCR()
        {
            initializeComplete = false;

            yield return SettingManager.InstanceCR(this);

            SystemManager.InstanceNW(this);
            LanguageManager.InstanceNW(this);
            AnchorManager.InstanceNW(this);

            initializeComplete = true;
        }
    }
}