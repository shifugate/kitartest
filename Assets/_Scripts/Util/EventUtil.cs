using System;
using UnityEngine;

namespace ARKit.Util
{
    public static class EventUtil
    {
        public static class Screen
        {
            public static Action<ContentUtil.Constant.Screen> LoadScreen;
        }

        public static class Setting 
        {
            public static Action SaveComplete;
        }

        public static class Anchror
        {
            public static Action CreateRoomComplete;
            public static Action RayOutOfRoom;
            public static Action RayInRoom;
            public static Action RayOutOfObject;
            public static Action<GameObject, float> RayInObject;
        }
    }
}
