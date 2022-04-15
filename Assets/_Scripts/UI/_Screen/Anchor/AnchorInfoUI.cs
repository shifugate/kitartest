using UnityEngine;
using UnityEngine.UI;

namespace KitAR.UI._Screen.Anchor
{
    public class AnchorInfoUI : MonoBehaviour
    {
        [SerializeField]
        private RectTransform rectTransform;
        [SerializeField]
        private Text infoText;

        public float Height { get { return rectTransform.rect.height; } }

        public void Setup(string info)
        {
            infoText.text = info;
        }
    }
}
