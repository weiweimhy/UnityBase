using UnityEngine;
using System.Runtime.InteropServices;

namespace BaseFramework.ThirdPlugin.Firebase
{
    public class FirebaseAnalyticsUtil
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        private static AndroidJavaClass utilClass;
#endif

        public static void Init()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            if (utilClass == null)
            {
                utilClass = new AndroidJavaClass("com.android4unity.util.firebase.AnalyticsUtil");
                utilClass.CallStatic("initAnalytics", AndroidNative.currentActivity);
            }
#endif
        }

        public static void LogEvent(string eventName)
        {
            Init();
#if UNITY_ANDROID && !UNITY_EDITOR
            utilClass.CallStatic("logEvent", eventName);
#elif UNITY_IOS && !UNITY_EDITOR
            logEvent(eventName);
#endif
        }

        public static void LogEvent(string eventName, string key, string value)
        {
            Init();
#if UNITY_ANDROID && !UNITY_EDITOR
            utilClass.CallStatic("logEvent", eventName, key, value);
#elif UNITY_IOS && !UNITY_EDITOR
            logEventString(eventName, key, value);
#endif
        }

        public static void LogEvent(string eventName, string key, int value)
        {
            Init();
#if UNITY_ANDROID && !UNITY_EDITOR
            utilClass.CallStatic("logEvent", eventName, key, value);
#elif UNITY_IOS && !UNITY_EDITOR
            logEventInt(eventName, key, value);
#endif
        }

        public static void LogEvent(string eventName, string key, float value)
        {
            Init();
#if UNITY_ANDROID && !UNITY_EDITOR
            utilClass.CallStatic("logEvent", eventName, key, value);
#elif UNITY_IOS && !UNITY_EDITOR
            logEventFloat(eventName, key, value);
#endif
        }

        public static void LogEvent(string eventName, string key, long value)
        {
            Init();
#if UNITY_ANDROID && !UNITY_EDITOR
            utilClass.CallStatic("logEvent", eventName, key, value);
#elif UNITY_IOS && !UNITY_EDITOR
            logEventLong(eventName, key, value);
#endif
        }

#if UNITY_IOS && !UNITY_EDITOR
	    [DllImport ("__Internal")]
	    public static extern void logEvent(string eventName);

	    [DllImport ("__Internal")]
	    public static extern void logEventString(string eventName, string key, string value);

        [DllImport ("__Internal")]
	    public static extern void logEventInt(string eventName, string key, int value);

        [DllImport ("__Internal")]
	    public static extern void logEventFloat(string eventName, string key, float value);

        [DllImport ("__Internal")]
	    public static extern void logEventLong(string eventName, string key, long value);
#endif
    }
}
