using System.Collections.Generic;
using UnityEngine;

namespace BaseFramework
{
    public class AndroidNative
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        private static AndroidJavaObject _currentActivity;
        public static AndroidJavaObject currentActivity
        {
            get
            {
                if (_currentActivity == null)
                {
                    _currentActivity = new AndroidJavaClass("com.unity3d.player.UnityPlayer")
                                            .GetStatic<AndroidJavaObject>("currentActivity");
                }
                return _currentActivity;
            }
        }
#endif

        public static AndroidJavaObject ConvertDictToHashMap(Dictionary<string, string> dict)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            if (dict == null)
            {
                return null;
            }
            AndroidJavaObject map = new AndroidJavaObject("java.util.HashMap");
            foreach (KeyValuePair<string, string> pair in dict)
            {
                map.Call<string>("put", pair.Key, pair.Value);
            }
            return map;
#else
            return null;
#endif
        }
    }
}
