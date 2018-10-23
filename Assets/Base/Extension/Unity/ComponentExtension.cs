using UnityEngine;

namespace Base.Extension
{
    public static class ComponentExtension 
    {
        public static T Active<T>(this T self) where T : Component
        {
            if (self && self.gameObject)
            {
                self.gameObject.Active();
            }
            return self;
        }

        public static T Inactive<T>(this T self) where T : Component
        {
            if (self)
            {
                self.gameObject.Inactive();
            }
            return self;
        }

        public static T Layer<T>(this T self, int layer) where T : Component
        {
            if (self && self.gameObject)
            {
                self.gameObject.Layer(layer);
            }
            return self;
        }

        public static T Layer<T>(this T self, string layerName) where T : Component
        {
            if (self && self.gameObject)
            {
                self.gameObject.Layer(layerName);
            }
            return self;
        }

        public static void DestroyGameObj<T>(this T self, float defaultDelay = 0) where T : Component
        {
            if (self && self.gameObject)
            {
                self.gameObject.Destroy();
            }
        }
    }
}
