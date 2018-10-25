using System;

namespace BaseFramework
{
    public static class Log
    {
        public enum LogLevel
        {
            VERBOSE = 2,
            DEBUG = 3,
            INFO = 4,
            WARN = 5,
            ERROR = 6,
            ASSERT = 7
        }

        public static LogLevel logLevel = LogLevel.VERBOSE;
        public static bool logUnityStack = false;

        private static string tagFormat = "[{0}]";

        private static void GetLogInfo(object obj, object msg, out string tag, out string message, params object[] args)
        {
            Type type = obj.GetType();

            if (obj is Type)
            {
                tag = tagFormat.Format((object)(obj as Type).Name);
            }
            else if (type.IsNotTypeof<string>())
            {
                tag = obj.GetLogTag();
            }
            else
            {
                tag = tagFormat.Format(obj);
            }
            tag = tag.AddSuffix(" ");

            message = msg.ToString().Format(args);
        }

        public static void V(object obj, object msg, params object[] args)
        {
            if (logLevel > LogLevel.VERBOSE)
            {
                return;
            }

            string tag = null;
            string message = null;
            GetLogInfo(obj, msg, out tag, out message, args);

            if (logUnityStack)
            {
                UnityEngine.Debug.Log(tag + " " + message);
            }
            else
            {
#if UNITY_EDITOR
                UnityEngine.Debug.Log(tag + " " + message);
#elif UNITY_ANDROID
                Log2Android.V(tag, message);
#elif UNITY_IOS
                Log2iOS.v(tag + " " + message);
#endif
            }
        }

       
        public static void D(object obj, object msg, params object[] args)
        {
            if (logLevel > LogLevel.DEBUG)
            {
                return;
            }

            string tag = null;
            string message = null;
            GetLogInfo(obj, msg, out tag, out message, args);

            if (logUnityStack)
            {
                UnityEngine.Debug.Log(tag + " " + message);
            }
            else
            {
#if UNITY_EDITOR
                UnityEngine.Debug.Log(tag + " " + message);
#elif UNITY_ANDROID
                Log2Android.D(tag, message);
#elif UNITY_IOS
                Log2iOS.d(tag + " " + message);
#endif
            }
        }

        public static void I(object obj, object msg, params object[] args)
        {
            if (logLevel > LogLevel.INFO)
            {
                return;
            }

            string tag = null;
            string message = null;
            GetLogInfo(obj, msg, out tag, out message, args);

            if (logUnityStack)
            {
                UnityEngine.Debug.Log(tag + " " + message);
            }
            else
            {
#if UNITY_EDITOR
                UnityEngine.Debug.Log(tag + " " + message);
#elif UNITY_ANDROID
                Log2Android.I(tag, message);
#elif UNITY_IOS
                Log2iOS.i(tag + " " + message);
#endif
            }
        }

        public static void W(object obj, object msg, params object[] args)
        {
            if (logLevel > LogLevel.WARN)
            {
                return;
            }

            string tag = null;
            string message = null;
            GetLogInfo(obj, msg, out tag, out message, args);

            if (logUnityStack)
            {
                UnityEngine.Debug.LogWarning(tag + " " + message);
            }
            else
            {
#if UNITY_EDITOR
                UnityEngine.Debug.LogWarning(tag + " " + message);
#elif UNITY_ANDROID
                Log2Android.W(tag, message);
#elif UNITY_IOS
                Log2iOS.w(tag + " " + message);
#endif
            }
        }

        public static void E(object obj, object msg, params object[] args)
        {
            if (logLevel > LogLevel.ERROR)
            {
                return;
            }

            string tag = null;
            string message = null;
            GetLogInfo(obj, msg, out tag, out message, args);

            if (logUnityStack)
            {

                UnityEngine.Debug.LogError(tag + " " + message);
            }
            else
            {
#if UNITY_EDITOR
                UnityEngine.Debug.LogError(tag + " " + message);
#elif UNITY_ANDROID
                Log2Android.E(tag, message);
#elif UNITY_IOS
                Log2iOS.e(tag + " " + message);
#endif
            }
        }

        public static void E(this Exception self, object msg, params object[] args)
        {
            E(self, msg, args);
        }
    }
}