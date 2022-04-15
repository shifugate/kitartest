using UnityEngine;
using KitAR.MVC._Base;

namespace KitAR.MVC.Home
{
    public class HomeModel : ModelBase
    {
        [SerializeField]
        private Transform uiHolder;
        public Transform UIHolder { get { return uiHolder; } }
    }
}
