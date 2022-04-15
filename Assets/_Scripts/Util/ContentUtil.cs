using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace KitAR.Util
{
    public static class ContentUtil
    {
        public static T Copy<T>(T data)
        {
            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(data));
        }

        public static async Task<IList<T>> LoadContents<T>(string key)
        {
            try
            {
                IList<T> asset = await Addressables.LoadAssetsAsync<T>(key, (result) => { }).Task;

                return asset;
            }
            catch (Exception)
            {

            }

            return default(IList<T>);
        }

        public static IEnumerator LoadContents<T>(string key, Action<IList<T>> completeCallback, Action<float> progressionCallback = null)
        {
            AsyncOperationHandle<IList<T>> handle = default(AsyncOperationHandle<IList<T>>);

            Exception error = null;

            try
            {
                handle = Addressables.LoadAssetsAsync<T>(key, (result) => { });
            }
            catch (Exception ex)
            {
                Debug.Log($"{key} : {ex}");

                error = ex;
            }

            if (error != null)
            {
                completeCallback?.Invoke(default(IList<T>));

                yield break;
            }

            while (handle.Status == AsyncOperationStatus.None)
            {
                progressionCallback?.Invoke(handle.PercentComplete);

                yield return null;
            }

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                completeCallback?.Invoke(handle.Result);

                yield break;
            }

            completeCallback?.Invoke(default(IList<T>));
        }

        public static async Task<T> LoadContent<T>(string file)
        {
            try
            {
                T asset = await Addressables.LoadAssetAsync<T>($"Assets/_Addressables/{file}").Task;

                return asset;
            }
            catch (Exception)
            {

            }

            return default(T);
        }

        public static IEnumerator LoadContent<T>(string file, Action<T> completeCallback, Action<float> progressionCallback = null)
        {
            AsyncOperationHandle<T> handle = default(AsyncOperationHandle<T>);

            Exception error = null;

            try
            {
                handle = Addressables.LoadAssetAsync<T>($"Assets/_Addressables/{file}");
            }
            catch(Exception ex)
            {
                Debug.Log($"{file} : {ex}");

                error = ex;
            }

            if (error != null)
            {
                completeCallback?.Invoke(default(T));

                yield break;
            }

            while (handle.Status == AsyncOperationStatus.None)
            {
                progressionCallback?.Invoke(handle.PercentComplete);

                yield return null;
            }

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                completeCallback?.Invoke(handle.Result);

                yield break;
            }

            completeCallback?.Invoke(default(T));
        }

        public static async Task<T> LoadContent<T>(string file, Transform parent)
        {
            GameObject go = await Addressables.InstantiateAsync($"Assets/_Addressables/{file}", parent).Task;

            if (go == null)
                return default(T);
            
            go.name = typeof(T).Name;

            if (typeof(T).Equals(typeof(GameObject)))
                return (T)Convert.ChangeType(go, typeof(T));

            return go.GetComponent<T>();
        }

        public static IEnumerator LoadContent<T>(string file, Transform parent, Action<T> completeCallback, Action<float> progressionCallback = null)
        {
            AsyncOperationHandle handle = Addressables.InstantiateAsync($"Assets/_Addressables/{file}", parent);

            while (handle.Status == AsyncOperationStatus.None)
            {
                progressionCallback?.Invoke(handle.PercentComplete);

                yield return null;
            }

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                GameObject go = (GameObject)handle.Result;
                
                if (go != null)
                    go.name = typeof(T).Name;

                if (typeof(T).Equals(typeof(GameObject)))
                    completeCallback?.Invoke((T)Convert.ChangeType(go, typeof(T)));
                else
                    completeCallback?.Invoke(go.GetComponent<T>());

                yield break;
            }

            completeCallback?.Invoke(default(T));
        }

        public static async Task<T> LoadContent<T>(string file, Vector3 position, Quaternion rotatation)
        {
            GameObject go = await Addressables.InstantiateAsync($"Assets/_Addressables/{file}", position, rotatation).Task;

            if (go == null)
                return default(T);

            go.name = typeof(T).Name;

            if (typeof(T).Equals(typeof(GameObject)))
                return (T)Convert.ChangeType(go, typeof(T));

            return go.GetComponent<T>();
        }

        public static IEnumerator LoadContent<T>(string file, Vector3 position, Quaternion rotatation, Action<T> completeCallback, Action<float> progressionCallback = null)
        {
            AsyncOperationHandle handle = Addressables.InstantiateAsync($"Assets/_Addressables/{file}", position, rotatation);

            while (handle.Status == AsyncOperationStatus.None)
            {
                progressionCallback?.Invoke(handle.PercentComplete);

                yield return null;
            }

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                GameObject go = (GameObject)handle.Result;

                if (go != null)
                    go.name = typeof(T).Name;

                if (typeof(T).Equals(typeof(GameObject)))
                    completeCallback?.Invoke((T)Convert.ChangeType(go, typeof(T)));
                else
                    completeCallback?.Invoke(go.GetComponent<T>());

                yield break;
            }

            completeCallback?.Invoke(default(T));
        }

        public static async Task<T> LoadContent<T>(string file, Vector3 position, Quaternion rotatation, Transform parent)
        {
            GameObject go = await Addressables.InstantiateAsync($"Assets/_Addressables/{file}", position, rotatation, parent).Task;

            if (go == null)
                return default(T);

            go.name = typeof(T).Name;

            if (typeof(T).Equals(typeof(GameObject)))
                return (T)Convert.ChangeType(go, typeof(T));

            return go.GetComponent<T>();
        }

        public static IEnumerator LoadContent<T>(string file, Vector3 position, Quaternion rotatation, Transform parent, Action<T> completeCallback, Action<float> progressionCallback = null)
        {
            AsyncOperationHandle handle = Addressables.InstantiateAsync($"Assets/_Addressables/{file}", position, rotatation, parent);

            while (handle.Status == AsyncOperationStatus.None)
            {
                progressionCallback?.Invoke(handle.PercentComplete);

                yield return null;
            }

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                GameObject go = (GameObject)handle.Result;

                if (go != null)
                    go.name = typeof(T).Name;

                if (typeof(T).Equals(typeof(GameObject)))
                    completeCallback?.Invoke((T)Convert.ChangeType(go, typeof(T)));
                else
                    completeCallback?.Invoke(go.GetComponent<T>());

                yield break;
            }

            completeCallback?.Invoke(default(T));
        }

        public static IEnumerator CaptureScreen(Action<Texture2D> completeCallback)
        {
            yield return new WaitForEndOfFrame();

            Texture2D texture = ScreenCapture.CaptureScreenshotAsTexture(3);

            if (texture.width > 1080)
            {
                float scale = 1080f / texture.width;

                int width = Mathf.FloorToInt(texture.width * scale);
                int height = Mathf.FloorToInt(texture.height * scale);

                texture = ScaleTexture(texture, width, height);
            }

            completeCallback?.Invoke(texture);
        }

        public static Texture2D ScaleTexture(Texture2D source, int targetWidth, int targetHeight)
        {
            Texture2D result = new Texture2D(targetWidth, targetHeight, source.format, true);

            Color[] rpixels = result.GetPixels(0);

            float incX = (1.0f / (float)targetWidth);
            float incY = (1.0f / (float)targetHeight);

            for (int px = 0; px < rpixels.Length; px++)
                rpixels[px] = source.GetPixelBilinear(incX * ((float)px % targetWidth), incY * ((float)Mathf.Floor(px / targetWidth)));

            result.SetPixels(rpixels, 0);
            result.Apply();

            return result;
        }

        public static class Constant
        {
            public enum Screen
            {
                Setting,
                Anchor
            }
        }
    }
}
