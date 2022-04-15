using UnityEngine;
using ARKit.MVC._Base;

namespace ARKit.MVC.Home
{
    public class HomeModel : ModelBase
    {
        [SerializeField]
        private Transform spaceHolder;
        public Transform SpaceHolder { get { return spaceHolder; } }

        [SerializeField]
        private Transform uiHolder;
        public Transform UIHolder { get { return uiHolder; } }
    }
}
