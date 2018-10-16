using System;
using System.Collections;
using System.Collections.Generic;

namespace Base.Log
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

        public static void V(this object self, object msg, params object[] @params)
        {
            V(self.GetType().Name, msg, @params);
        }

        public static void V(string tag, object msg, params object[] @params)
        {
            if (logLevel > LogLevel.VERBOSE)
            {
                return;
            }

            tag = string.Format("[{0}]", GetReadabilityString(tag));
            string message = msg.ToString();
            if (@params != null && @params.Length > 0)
            {
                message = string.Format(message, @params);
            }

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
#endif
            }
        }

        public static void D(this object self, object msg, params object[] @params)
        {
            D(self.GetType().Name, msg, @params);
        }

        public static void D(string tag, object msg, params object[] @params)
        {
            if (logLevel > LogLevel.DEBUG)
            {
                return;
            }

            tag = string.Format("[{0}]", GetReadabilityString(tag));
            string message = msg.ToString();
            if (@params != null && @params.Length > 0)
            {
                message = string.Format(message, @params);
            }

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
#endif
            }
        }

        public static void I(this object self, object msg, params object[] @params)
        {
            I(self.GetType().Name, msg, @params);
        }

        public static void I(string tag, object msg, params object[] @params)
        {
            if (logLevel > LogLevel.INFO)
            {
                return;
            }

            tag = string.Format("[{0}]", GetReadabilityString(tag));
            string message = msg.ToString();
            if (@params != null && @params.Length > 0)
            {
                message = string.Format(message, @params);
            }

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
#endif
            }
        }

        public static void W(this object self, object msg, params object[] @params)
        {
            W(self.GetType().Name, msg, @params);
        }

        public static void W(string tag, object msg, params object[] @params)
        {
            if (logLevel > LogLevel.WARN)
            {
                return;
            }

            tag = string.Format("[{0}]", GetReadabilityString(tag));
            string message = msg.ToString();
            if (@params != null && @params.Length > 0)
            {
                message = string.Format(message, @params);
            }

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
#endif
            }
        }

        public static void E(this object self, object msg, params object[] @params)
        {
            E(self.GetType().Name, msg, @params);
        }

        public static void E(string tag, object msg, params object[] @params)
        {
            if (logLevel > LogLevel.ERROR)
            {
                return;
            }

            tag = string.Format("[{0}]", GetReadabilityString(tag));
            string message = msg.ToString();
            if (@params != null && @params.Length > 0)
            {
                message = string.Format(message, @params);
            }

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
#endif
            }
        }

        public static void E(this Exception self, object msg, params object[] @params)
        {
            E(self.GetType().Name, msg, @params);
        }

        private static string GetReadabilityString(string str)
        {
            if (str == null)
                return "@NULL";

            if (str.Length == 0)
                return "@EMPTY";

            return str;
        }
    }
}