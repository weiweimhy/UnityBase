using UnityEngine;

namespace BaseFramework
{
    public static class BehaviourExtension
    {
        public static T Enable<T>(this T self) where T : Behaviour
        {
            if (self)
            {
                self.enabled = true;
            }
            return self;
        }

        public static T Disable<T>(this T self) where T : Behaviour
        {
            if (self)
            {
                self.enabled = false;
            }
            return self;
        }
    }
}