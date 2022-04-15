using ARKit.Helper.Camera;
using ARKit.Manager.Setting;
using ARKit.Util;
using UnityEngine;

namespace ARKit.Helper.Anchor
{
    public class AnchorHelper : MonoBehaviour
    {
        [SerializeField]
        private Light ligth;

        private CameraHelper cameraHelper;
        private float roomSize;
        private float anchorReach;
        private bool inRoom;

        private void Awake()
        {
            SetProperties();
            SetupRoom();
            SetupLight();
        }

        private void OnDestroy()
        {
            cameraHelper.transform.position = Vector3.zero;
            cameraHelper.transform.rotation = Quaternion.identity;

            Destroy(cameraHelper);
        }

        private void Update()
        {
            UpdateRayInRoom();
            UpdateRayInObject();
        }

        private void SetProperties()
        {
            cameraHelper = UnityEngine.Camera.main.gameObject.AddComponent<CameraHelper>();

            roomSize = SettingManager.Instance.DataCurrent.room_size / 100f;
            anchorReach = SettingManager.Instance.DataCurrent.anchor_reach / 100f;
        }

        private async void SetupRoom()
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

            Debug.Log(position + " : " + distance + " : " + roomSize);

            if (distance <= roomSize / 2f)
            {
                inRoom = true;

                EventUtil.Anchror.RayInRoom?.Invoke();
            }
            else if (distance > roomSize / 2f)
            {
                inRoom = false;

                EventUtil.Anchror.RayOutOfRoom?.Invoke();
            }
        }

        private void UpdateRayInObject()
        {

        }
    }
}
