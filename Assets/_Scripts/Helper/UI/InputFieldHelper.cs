using UnityEngine;
using UnityEngine.UI;

namespace ARKit.Helper.UI
{
    public class InputFieldHelper : MonoBehaviour
    {
        [SerializeField]
        private Text labeltext;
        [SerializeField]
        private InputField inputField;
        [SerializeField]
        private Text errorText;

        public string text 
        {
            get
            {
                return inputField.text;
            }

            set
            {
                inputField.text = value != null ? value : "";
            }
        }

        private void Awake()
        {
            errorText.enabled = false;
        }

        public void SetLabel(string label)
        {
            labeltext.text = label;
        }

        public void SetError(string error)
        {
            errorText.text = error;
            errorText.enabled = error != null;
        }
    }
}
