﻿using KitAR.Helper.Anchor;
using KitAR.Helper.Camera;
using KitAR.Manager.Setting;
using KitAR.Util;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace KitAR.Manager.Anchor
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
        [SerializeField]
        private Transform contenHolder;

        private Coroutine updatetDetectionCR;
        private CameraHelper cameraHelper;
        private List<AnchorHelper> anchors = new List<AnchorHelper>();
        private AnchorHelper anchor;
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
            if (instance == null)
                return;
            
            if (updatetDetectionCR != null)
                StopCoroutine(updatetDetectionCR);

            if (cameraHelper != null)
            {
                cameraHelper.transform.position = Vector3.zero;
                cameraHelper.transform.rotation = Quaternion.identity;

                Destroy(cameraHelper);
            }

            foreach (Transform transform in contenHolder)
                Destroy(transform.gameObject);

            anchors.Clear();
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

            anchor = null;

            roomSize = SettingManager.Instance.DataCurrent.room_size / 100f;
            anchorReach = SettingManager.Instance.DataCurrent.anchor_reach / 100f;

            cameraHelper = Camera.main.gameObject.AddComponent<CameraHelper>();
            cameraHelper.transform.position = Vector3.zero;
            cameraHelper.transform.rotation = Quaternion.identity;
        }

        private async Task SetupRoom()
        {
            GameObject roof = await ContentUtil.LoadContent<GameObject>("Helper/Wall/WallHelper.prefab", contenHolder);
            roof.name = "Roof";

            GameObject floor = await ContentUtil.LoadContent<GameObject>("Helper/Wall/WallHelper.prefab", contenHolder);
            floor.name = "Floor";

            GameObject leftWall = await ContentUtil.LoadContent<GameObject>("Helper/Wall/WallHelper.prefab", contenHolder);
            leftWall.name = "LeftWall";

            GameObject rightWall = await ContentUtil.LoadContent<GameObject>("Helper/Wall/WallHelper.prefab", contenHolder);
            rightWall.name = "RightWall";

            GameObject frontWall = await ContentUtil.LoadContent<GameObject>("Helper/Wall/WallHelper.prefab", contenHolder);
            frontWall.name = "FrontWall";

            GameObject backWall = await ContentUtil.LoadContent<GameObject>("Helper/Wall/WallHelper.prefab", contenHolder);
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

            float distanceReticle = Vector3.Distance(Vector3.zero, position);
            float distanceCamera = Vector2.Distance(Vector3.zero, cameraHelper.transform.position);

            if (!inRoom 
                && distanceReticle <= roomSize / 2f
                && distanceCamera <= roomSize / 2f)
            {
                inRoom = true;

                EventUtil.Anchror.RayInRoom?.Invoke();
            }
            else if (inRoom
                && (distanceReticle > roomSize / 2f
                || distanceCamera > roomSize / 2f))
            {
                inRoom = false;

                EventUtil.Anchror.RayOutOfRoom?.Invoke();
            }
        }

        private void UpdateRayInObject()
        {
            if (Physics.Raycast(cameraHelper.transform.position, cameraHelper.transform.forward, out RaycastHit hit, anchorReach))
            {
                AnchorHelper anchor = hit.collider.gameObject.GetComponent<AnchorHelper>();

                if (this.anchor != anchor)
                {
                    if (this.anchor != null)
                        this.anchor.Unselected();

                    this.anchor = anchor;
                    this.anchor.Selected();

                    EventUtil.Anchror.RayInObject?.Invoke(anchor);
                }
            }
            else if (this.anchor != null)
            {
                this.anchor.Unselected();
                this.anchor = null;

                EventUtil.Anchror.RayOutOfObject?.Invoke();
            }
        }

        public void Enable()
        {
            StartDetection();
        }

        public void Disable()
        {
            StopDetection();
        }

        public async void AddAnchor()
        {
            Vector3 position = cameraHelper.transform.position + cameraHelper.transform.forward * anchorReach;

            AnchorHelper anchor = await ContentUtil.LoadContent<AnchorHelper>("Helper/Anchor/AnchorHelper.prefab", contenHolder);

            if (anchor == null)
                return;

            anchor.transform.position = position;

            EventUtil.Anchror.CreateAnchorComplete?.Invoke();
        }

        public void RemoveAnchor(AnchorHelper anchor)
        {
            if (anchor != null)
                Destroy(anchor.gameObject);
        }
    }
}
