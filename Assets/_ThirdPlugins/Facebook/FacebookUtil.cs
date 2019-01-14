using System.Collections.Generic;
using UnityEngine;

namespace BaseFramework.ThirdPlugin.Facebook
{
    public class FacebookUtil
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        private static AndroidJavaClass _utilClass;
        private static AndroidJavaClass utilClass
        {
            get
            {
                if (_utilClass == null)
                {
                    _utilClass = new AndroidJavaClass("com.android4unity.util.facebook.Util");
                }
                return _utilClass;
            }
        }
#endif

        public static void LogEvent(string eventName)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            utilClass.CallStatic("logEvent", AndroidNative.currentActivity, eventName);
#endif
        }

        public static void LogEvent(string eventName, double value)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            utilClass.CallStatic("logEvent", AndroidNative.currentActivity, eventName, value);
#endif
        }

        public static void LogEvent(string eventName, Dictionary<string, string> parameters)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            utilClass.CallStatic("logEvent", AndroidNative.currentActivity, eventName, AndroidNative.ConvertDictToHashMap(parameters));
#endif
        }

        public static void LogEvent(string eventName, double value, Dictionary<string, string> parameters)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            utilClass.CallStatic("logEvent", AndroidNative.currentActivity, eventName, value, AndroidNative.ConvertDictToHashMap(parameters));
#endif
        }
    }
}
