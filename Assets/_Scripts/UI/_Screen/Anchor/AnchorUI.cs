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

        private bool createRoomComplete;

        private void Awake()
        {
            AddListener();

            AnchorManager.Instance.Enable();
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
            EventUtil.Anchror.RayInRoom += RayInRoom;
            EventUtil.Anchror.RayOutOfRoom += RayOutOfRoom;
        }

        private void RemoveListener() 
        {
            EventUtil.Anchror.CreateRoomComplete -= CreateRoomComplete;
            EventUtil.Anchror.RayInRoom -= RayInRoom;
            EventUtil.Anchror.RayOutOfRoom -= RayOutOfRoom;
        }

        private void UpdateCursor()
        {
            Cursor.visible = false;
        }

        private void CreateRoomComplete()
        {
            createRoomComplete = true;

            SetInteractionText();
        }

        private void RayInRoom()
        {
            reticleImage.color = Color.yellow;
        }

        private void RayOutOfRoom()
        {
            reticleImage.color = Color.red;
        }

        private void SetInteractionText()
        {
            string interactionText = $"{LanguageManager.Instance.GetTranslation("common", "move_interaction_token")}\n" +
                $"{LanguageManager.Instance.GetTranslation("common", "look_interaction_token")}\n" +
                $"{LanguageManager.Instance.GetTranslation("common", "exit_interaction_token")}\n" +
                $"{LanguageManager.Instance.GetTranslation("common", "add_interaction_token")}";

            this.interactionText.text = interactionText;
        }

        private void UpdateInput()
        {
            if (!createRoomComplete)
                return;

            if (Input.GetKeyDown(KeyCode.P))
                EventUtil.Screen.LoadScreen?.Invoke(ContentUtil.Constant.Screen.Setting);

            if (Input.GetKeyDown(KeyCode.Space))
                AnchorManager.Instance.AddAnchor();
        }
    }
}
