using UnityEngine;

namespace ARKit.MVC._Base
{
    public class ControllerBase<TView, TModel> : MonoBehaviour
    {
        private TView view;
        public TView View
        {
            get
            {
                if (view == null)
                    view = GetComponent<TView>();

                return view;
            }
        }

        private TModel model;
        public TModel Model
        {
            get
            {
                if (model == null)
                    model = GetComponent<TModel>();

                return model;
            }
        }
    }
}
