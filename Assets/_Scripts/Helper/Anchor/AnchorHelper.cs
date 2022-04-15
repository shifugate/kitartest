using UnityEngine;

namespace KitAR.Helper.Anchor
{
    public class AnchorHelper : MonoBehaviour
    {
        [SerializeField]
        private Renderer anchorRenderer;

        private void Awake()
        {
            Unselected();
        }

        public void Selected()
        {
            anchorRenderer.material.color = Color.green;
        }

        public void Unselected()
        {
            anchorRenderer.material.color = Color.red;
        }
    }
}
