using UnityEngine;
using System.Runtime.InteropServices;

namespace BaseFramework.ThirdPlugin.Firebase
{
    public class FirebaseCrashlyticsUtil
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        private static AndroidJavaClass _utilClass;
        private static AndroidJavaClass utilClass
        {
            get
            {
                if(_utilClass == null) 
                {
                    _utilClass = new AndroidJavaClass("com.android4unity.util.firebase.CrashlyticsUtil");
                }
                return _utilClass;
            }
        }

        private static AndroidJavaClass _crashlyticsClass;
        private static AndroidJavaClass crashlyticsClass
        {
            get
            {
                if(_crashlyticsClass == null) 
                {
                    _crashlyticsClass = new AndroidJavaClass("com.crashlytics.android.Crashlytics");
                }
                return _crashlyticsClass;
            }
        }

        private static bool initFinished = false;
#endif

        public static void Init()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            if(!initFinished) {
                initFinished = true;
                UnityCrashlytics.Init();
                UnityCrashlytics.onPostException += (condition, stackTrace) => {
                    utilClass.CallStatic("postException", condition, stackTrace);
                };
            }
#endif
        }

        public static void TestCrash()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            utilClass.CallStatic("testCrash");
#elif UNITY_IOS && !UNITY_EDITOR
            testCrash();
#endif
        }

        public static void LogEvent(string msg)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            crashlyticsClass.CallStatic("log", msg);
#elif UNITY_IOS && !UNITY_EDITOR
            logEvent(msg);
#endif
        }

        /// <summary>
        /// VERBOSE = 2
        /// DEBUG = 3
        /// INFO = 4
        /// WARN = 5
        /// ERROR = 6
        /// ASSERT = 7
        /// </summary>
        /// <param name="priority"></param>
        /// <param name="tag"></param>
        /// <param name="msg"></param>
        public static void LogEvent(int priority, string tag, string msg)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            crashlyticsClass.CallStatic("log", priority, tag, msg);
#elif UNITY_IOS && !UNITY_EDITOR
            Log.I("FirebaseCrashlyticsUtil", "Log event, msg:" + tag + ", " + msg);
            logEvent(msg);
#endif
        }

        public static void SetValue(string key, string value)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            crashlyticsClass.CallStatic("setString", key, value);
#elif UNITY_IOS && !UNITY_EDITOR
            setString(key, value);
#endif
        }

        public static void SetValue(string key, bool value)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            crashlyticsClass.CallStatic("setBool", key, value);
#elif UNITY_IOS && !UNITY_EDITOR
            setBool(key, value);
#endif
        }

        public static void SetValue(string key, double value)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            crashlyticsClass.CallStatic("setDouble", key, value);
#elif UNITY_IOS && !UNITY_EDITOR
            setFloat(key, (float)value);
#endif
        }

        public static void SetValue(string key, float value)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            crashlyticsClass.CallStatic("setFloat", key, value);
#elif UNITY_IOS && !UNITY_EDITOR
            setFloat(key, value);
#endif
        }

        public static void SetValue(string key, int value)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            crashlyticsClass.CallStatic("setInt", key, value);
#elif UNITY_IOS && !UNITY_EDITOR
            setInt(key, value);
#endif
        }

        public static void SetUserIdentifier(string id)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            crashlyticsClass.CallStatic("setUserIdentifier", id);
#elif UNITY_IOS && !UNITY_EDITOR
            setUserIdentifier(id);
#endif
        }

        public static void PostException(string msg, string stackTrace)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            crashlyticsClass.CallStatic("postException", msg, stackTrace);
#elif UNITY_IOS && !UNITY_EDITOR
            postException(msg + "\n" + stackTrace);
#endif
        }

#if UNITY_IOS && !UNITY_EDITOR
	    [DllImport ("__Internal")]
	    public static extern void testCrash();

        [DllImport ("__Internal")]
	    public static extern void logEvent(string msg);
        
        [DllImport ("__Internal")]
	    public static extern void setString(string key, string value);

        [DllImport ("__Internal")]
	    public static extern void setBool(string key, bool value);

        [DllImport ("__Internal")]
	    public static extern void setFloat(string key, float value);

        [DllImport ("__Internal")]
	    public static extern void setInt(string key, int value);
        
        [DllImport ("__Internal")]
	    public static extern void setUserIdentifier(string id);  
        
        [DllImport ("__Internal")]
	    public static extern void postException(string msg);
#endif
    }
}
