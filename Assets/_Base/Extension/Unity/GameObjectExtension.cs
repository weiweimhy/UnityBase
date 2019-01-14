using UnityEngine;

namespace BaseFramework
{
    public static class GameObjectExtension
    {
        public static GameObject Active(this GameObject self)
        {
            if (self)
            {
                self.SetActive(true);
            }
            return self;
        }

        public static GameObject Inactive(this GameObject self)
        {
            if (self)
            {
                self.SetActive(false);
            }
            return self;
        }

        public static GameObject Layer(this GameObject self, int layer)
        {
            if(self)
            {
                self.layer = layer;
            }
            return self;
        }

        public static GameObject Layer(this GameObject self, string layerName)
        {
            if (self)
            {
                self.layer = LayerMask.NameToLayer(layerName);
            }
            return self;
        }

        public static T GetComponentNotNull<T>(this GameObject self) where T:Component
        {
            if(!self)
            {
                throw new System.NullReferenceException("Null object can not get component!");
            }

            T component = self.GetComponent<T>();
            return component ? component : self.AddComponent<T>();
        }
    }
}