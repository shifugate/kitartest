using KitAR.Helper.Anchor;
using KitAR.Manager.Anchor;
using KitAR.Manager.Language;
using KitAR.Util;
using UnityEngine;
using UnityEngine.UI;

namespace KitAR.UI._Screen.Anchor
{
    public class AnchorUI : MonoBehaviour
    {
        [SerializeField]
        private RectTransform interactionHolder;
        [SerializeField]
        private Text interactionText;
        [SerializeField]
        private Image reticleImage;

        private AnchorHelper anchor;
        private bool createRoomComplete;
        private bool createAnchorComplete;
        private bool inRoom;

        private void Awake()
        {
            AddListener();

            AnchorManager.Instance.Enable();

            createAnchorComplete = true;
        }

        private void OnDestroy()
        {
            RemoveListener();

            AnchorManager.Instance.Disable();

            Cursor.visible = true;
        }

        private void Update()
        {
            UpdateInput();
            UpdateCursor();
        }

        private void AddListener()
        {
            EventUtil.Anchror.CreateRoomComplete += CreateRoomComplete;
            EventUtil.Anchror.CreateAnchorComplete += CreateAnchorComplete;
            EventUtil.Anchror.RayInRoom += RayInRoom;
            EventUtil.Anchror.RayOutOfRoom += RayOutOfRoom;
            EventUtil.Anchror.RayInObject += RayInObject;
            EventUtil.Anchror.RayOutOfObject += RayOutOfObject;
        }

        private void RemoveListener() 
        {
            EventUtil.Anchror.CreateRoomComplete -= CreateRoomComplete;
            EventUtil.Anchror.CreateAnchorComplete -= CreateAnchorComplete;
            EventUtil.Anchror.RayInRoom -= RayInRoom;
            EventUtil.Anchror.RayOutOfRoom -= RayOutOfRoom;
            EventUtil.Anchror.RayInObject -= RayInObject;
            EventUtil.Anchror.RayOutOfObject -= RayOutOfObject;
        }

        private void UpdateCursor()
        {
            Cursor.visible = false;
        }

        private void CreateRoomComplete()
        {
            createRoomComplete = true;

            SetInteractionText(LanguageManager.Instance.GetTranslation("common", "add_interaction_token"));
        }

        private void CreateAnchorComplete()
        {
            createAnchorComplete = true;
        }

        private void RayInRoom()
        {
            inRoom = true;

            reticleImage.color = Color.green;
        }

        private void RayOutOfRoom()
        {
            inRoom = false;

            reticleImage.color = Color.red;
        }

        private void RayInObject(AnchorHelper anchor)
        {
            this.anchor = anchor;

            SetInteractionText(LanguageManager.Instance.GetTranslation("common", "remove_interaction_token"));
        }

        private void RayOutOfObject()
        {
            this.anchor = null;

            SetInteractionText(LanguageManager.Instance.GetTranslation("common", "add_interaction_token"));
        }

        private void SetInteractionText(string anchorMessage)
        {
            string interactionText = $"{LanguageManager.Instance.GetTranslation("common", "move_interaction_token")}\n" +
                $"{LanguageManager.Instance.GetTranslation("common", "look_interaction_token")}\n" +
                $"{LanguageManager.Instance.GetTranslation("common", "exit_interaction_token")}";

            if (anchorMessage != null)
                interactionText += $"\n{anchorMessage}";

            this.interactionText.text = interactionText;
        }

        private void UpdateInput()
        {
            if (!createRoomComplete)
                return;

            if (Input.GetKeyDown(KeyCode.P))
                EventUtil.Screen.LoadScreen?.Invoke(ContentUtil.Constant.Screen.Setting);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (inRoom
                    && createAnchorComplete
                    && anchor == null)
                {
                    createAnchorComplete = false;

                    AnchorManager.Instance.AddAnchor();
                }
                else if (createAnchorComplete 
                    && anchor != null)
                {
                    AnchorManager.Instance.RemoveAnchor(anchor);
                }
            }
        }
    }
}
