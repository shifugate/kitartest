using UnityEngine;

namespace ARKit.Helper.Camera
{
    public class CameraHelper : MonoBehaviour
    {
        public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
        private RotationAxes axes = RotationAxes.MouseXAndY;

        private const float sensitivityX = 2F;
        private const float sensitivityY = 2F;
        private const float minimumY = -90F;
        private const float maximumY = 90F;
        private const float moveSpeed = 0.01f;

        private float rotationY = -60F;

        private void Update()
        {
            RotateCamera();
            MoveCamera();
        }

        private void RotateCamera()
        {
            if (axes == RotationAxes.MouseXAndY)
            {
                float rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityX;

                rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
                rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

                transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
            }
            else if (axes == RotationAxes.MouseX)
            {
                transform.Rotate(0, Input.GetAxis("Mouse X") * sensitivityX, 0);
            }
            else
            {
                rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
                rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

                transform.localEulerAngles = new Vector3(-rotationY, transform.localEulerAngles.y, 0);
            }
        }

        private void MoveCamera()
        {
            Vector3 pos = transform.position;

            if (Input.GetKey(KeyCode.W))
            {
                pos = pos + transform.forward * moveSpeed;

                transform.position = pos;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                pos = pos - transform.forward * moveSpeed;

                transform.position = pos;
            }

            if (Input.GetKey(KeyCode.D))
            {
                pos = pos + transform.right * moveSpeed;

                transform.position = pos;
            }
            else if (Input.GetKey(KeyCode.A))
            {
                pos = pos - transform.right * moveSpeed;

                transform.position = pos;
            }
        }
    }
}
