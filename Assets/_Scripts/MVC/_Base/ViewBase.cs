using UnityEngine;

namespace KitAR.MVC._Base
{
    public class ViewBase<TController, TModel> : MonoBehaviour
    {
        private TController controller;
        public TController Controller
        {
            get
            {
                if (controller == null)
                    controller = GetComponent<TController>();

                return controller;
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
