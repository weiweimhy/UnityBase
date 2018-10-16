
#if UNITY_ANDROID && !UNITY_EDITOR
using UnityEngine;

namespace Base.Log
{
    public class Log2Android
    {
        static readonly string ANDROID_HELPER_PACKAGE = "com.android.helper4unity";

        static readonly string LOG_CLASS = "Log";
        static AndroidJavaClass androidLogClass;

        static void InitLogClass()
        {
            if (androidLogClass == null)
                androidLogClass = new AndroidJavaClass(ANDROID_HELPER_PACKAGE + "." + LOG_CLASS);
        }

        public static void V(string tag, string message)
        {
            InitLogClass();
            androidLogClass.CallStatic("v", tag, message);
        }

        public static void D(string tag, string message)
        {
            InitLogClass();
            androidLogClass.CallStatic("d", tag, message);
        }

        public static void I(string tag, string message)
        {
            InitLogClass();
            androidLogClass.CallStatic("i", tag, message);
        }

        public static void W(string tag, string message)
        {
            InitLogClass();
            androidLogClass.CallStatic("w", tag, message);
        }

        public static void E(string tag, string message)
        {
            InitLogClass();
            androidLogClass.CallStatic("e", tag, message);
        }

    }
}

#endif