using ARKit.Helper.Camera;
using ARKit.Manager.Setting;
using ARKit.Util;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

namespace ARKit.Manager.Anchor
{
    public class AnchorManager : MonoBehaviour
    {
        #region Singleton
        private static AnchorManager instance;
        public static AnchorManager Instance { get { return instance; } }

        public static void InstanceNW(InitializerManager manager)
        {
            if (instance == null)
            {
                instance = GameObject.Instantiate<AnchorManager>(Resources.Load<AnchorManager>("Manager/Anchor/AnchorManager"));
                instance.name = "AnchorManager";
            }

            instance.Initialize(manager);
        }
        #endregion 

        [SerializeField]
        private Light ligth;

        private Coroutine updatetDetectionCR;
        private CameraHelper cameraHelper;
        private float roomSize;
        private float anchorReach;
        private bool inRoom;

        private void Initialize(InitializerManager manager)
        {
            transform.SetParent(manager.transform);
        }

        private async void StartDetection()
        {
            StopDetection();
            SetContent();

            await SetupRoom();

            SetupLight();

            updatetDetectionCR = StartCoroutine(UpdatetDetectionCR());
        }

        private void StopDetection()
        {
            if (instance != null 
                && updatetDetectionCR != null)
                StopCoroutine(updatetDetectionCR);

            if (instance != null
                && cameraHelper != null)
            {
                cameraHelper.transform.position = Vector3.zero;
                cameraHelper.transform.rotation = Quaternion.identity;

                Destroy(cameraHelper);
            }
        }

        private IEnumerator UpdatetDetectionCR()
        {
            while (true)
            {
                yield return new WaitForEndOfFrame();

                UpdateRayInRoom();
                UpdateRayInObject();
            }
        }

        private void SetContent()
        {
            inRoom = false;

            roomSize = SettingManager.Instance.DataCurrent.room_size / 100f;
            anchorReach = SettingManager.Instance.DataCurrent.anchor_reach / 100f;

            cameraHelper = Camera.main.gameObject.AddComponent<CameraHelper>();
            cameraHelper.transform.position = Vector3.zero;
            cameraHelper.transform.rotation = Quaternion.identity;
        }

        private async Task SetupRoom()
        {
            GameObject roof = await ContentUtil.LoadContent<GameObject>("Helper/Wall/WallHelper.prefab", transform);
            roof.name = "Roof";

            GameObject floor = await ContentUtil.LoadContent<GameObject>("Helper/Wall/WallHelper.prefab", transform);
            floor.name = "Floor";

            GameObject leftWall = await ContentUtil.LoadContent<GameObject>("Helper/Wall/WallHelper.prefab", transform);
            leftWall.name = "LeftWall";

            GameObject rightWall = await ContentUtil.LoadContent<GameObject>("Helper/Wall/WallHelper.prefab", transform);
            rightWall.name = "RightWall";

            GameObject frontWall = await ContentUtil.LoadContent<GameObject>("Helper/Wall/WallHelper.prefab", transform);
            frontWall.name = "FrontWall";

            GameObject backWall = await ContentUtil.LoadContent<GameObject>("Helper/Wall/WallHelper.prefab", transform);
            backWall.name = "BackWall";

            roof.transform.localScale = new Vector3(roomSize, 0.001f, roomSize);
            roof.transform.localPosition = new Vector3(0, roomSize / 2, 0);

            floor.transform.localScale = new Vector3(roomSize, 0.001f, roomSize);
            floor.transform.localPosition = new Vector3(0, -roomSize / 2, 0);

            leftWall.transform.localScale = new Vector3(roomSize, roomSize, 0.001f);
            leftWall.transform.localEulerAngles = new Vector3(0, 90, 0);
            leftWall.transform.localPosition = new Vector3(-roomSize / 2, 0, 0);

            rightWall.transform.localScale = new Vector3(roomSize, roomSize, 0.001f);
            rightWall.transform.localEulerAngles = new Vector3(0, -90, 0);
            rightWall.transform.localPosition = new Vector3(roomSize / 2, 0, 0);

            frontWall.transform.localScale = new Vector3(roomSize, roomSize, 0.001f);
            frontWall.transform.localPosition = new Vector3(0, 0, roomSize / 2);

            backWall.transform.localScale = new Vector3(roomSize, roomSize, 0.001f);
            backWall.transform.localPosition = new Vector3(0, 0, -roomSize / 2);

            EventUtil.Anchror.CreateRoomComplete?.Invoke();
        }

        private void SetupLight()
        {
            float intensity = roomSize / 3 * roomSize / 3;

            ligth.range = roomSize * 100;
            ligth.intensity = intensity;
        }

        private void UpdateRayInRoom()
        {
            Vector3 position = cameraHelper.transform.position + cameraHelper.transform.forward * anchorReach;

            float distance = Vector3.Distance(Vector3.zero, position);

            if (!inRoom 
                && distance <= roomSize / 2f)
            {
                inRoom = true;

                EventUtil.Anchror.RayInRoom?.Invoke();
            }
            else if (inRoom
                && distance > roomSize / 2f)
            {
                inRoom = false;

                EventUtil.Anchror.RayOutOfRoom?.Invoke();
            }
        }

        private void UpdateRayInObject()
        {

        }

        public void Enable()
        {
            StartDetection();
        }

        public void Disable()
        {
            StopDetection();
        }
    }
}
