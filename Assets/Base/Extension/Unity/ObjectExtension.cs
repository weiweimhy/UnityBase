using UnityEngine;

namespace BaseFramework
{
    public static class ObjectExtension
    {
        public static T Instantiate<T>(this T selfObj) where T : Object
        {
            if (selfObj)
                return Object.Instantiate(selfObj);
            return null;
        }

        public static T Name<T>(this T self, string name) where T : Object
        {
            if(self)
                self.name = name;
            return self;
        }

        public static void Destroy<T>(this T self, float defaultDelay = 0) where T : Object
        {
            if(self)
                Object.Destroy(self, defaultDelay);
        }

        public static T DontDestroyOnLoad<T>(this T self) where T : Object
        {
            Object.DontDestroyOnLoad(self);
            return self;
        }

        public static T As<T>(this Object self) where T : Object
        {
            return self as T;
        }
    }
}