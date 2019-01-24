using UnityEngine;
using System.Runtime.InteropServices;

namespace BaseFramework.ThirdPlugin.Bugly
{
    public class BuglyUtil
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        private static AndroidJavaClass _utilClass;
        private static AndroidJavaClass utilClass
        {
            get
            {
                if (_utilClass == null)
                {
                    _utilClass = new AndroidJavaClass("com.android4unity.util.bugly.CrashlyticsUtil");
                }
                return _utilClass;
            }
        }

        private static AndroidJavaClass _buglyClass;
        public static AndroidJavaClass buglyClass
        {
            get
            {
                if (_buglyClass == null)
                {
                    _buglyClass = new AndroidJavaClass("com.tencent.bugly.crashreport.CrashReport");
                }
                return _buglyClass;
            }
        }

        private static AndroidJavaClass _buglyLogClass;
        public static AndroidJavaClass buglyLogClass
        {
            get
            {
                if (_buglyLogClass == null)
                {
                    _buglyLogClass = new AndroidJavaClass("com.tencent.bugly.crashreport.BuglyLog");
                }
                return _buglyLogClass;
            }
        }
#endif

        private static bool initFinished = false;

        public static void Init(string id, bool isDebug, bool enableExceptionHandler = true)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            if (!initFinished)
            {
                initFinished = true;
                utilClass.CallStatic("init", AndroidNative.currentActivity, id, isDebug);

                if (enableExceptionHandler)
                {
                    EnableExceptionHandler();
                }
            }
#elif UNITY_IOS && !UNITY_EDITOR
            if (!initFinished)
            {
                initFinished = true;

                bugly_init(id, isDebug, 5, true, 3f);

                if (enableExceptionHandler)
                {
                    EnableExceptionHandler();
                }
            }
#endif
        }


        public static void Init(string id, bool isDebug, int delay, string channel,
                                string versionName, string packageName, bool enableExceptionHandler = true)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            if (!initFinished)
            {
                initFinished = true;
                _utilClass.CallStatic("init", AndroidNative.currentActivity, id, isDebug, delay, channel, versionName, packageName);

                if (enableExceptionHandler)
                {
                    EnableExceptionHandler();
                }
            }
#elif UNITY_IOS && !UNITY_EDITOR
            if (!initFinished)
            {
                initFinished = true;

                bugly_init(id, isDebug, 5, true, 3f);

                if (enableExceptionHandler)
                {
                    EnableExceptionHandler();
                }
            }
#endif
        }

        private static void EnableExceptionHandler()
        {
#if !UNITY_EDITOR
            UnityCrashlytics.Init();
            UnityCrashlytics.onPostException += PostException;
#endif
        }

        public static void AddExtraData(string msg)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            utilClass.CallStatic("addExtraData", msg);
#elif UNITY_IOS && !UNITY_EDITOR
            bugly_addExtraData(msg);
#endif
        }

        public static void PostException(string msg, string stackTrace)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            utilClass.CallStatic("postException", msg, stackTrace);
#elif UNITY_IOS && !UNITY_EDITOR
            if (!string.IsNullOrEmpty(msg))
            {
                if (msg.Contains("\n"))
                {
                    string[] temp = msg.Split('\n');
                    bugly_reportException(temp[0], temp[1], stackTrace);
                }
                else if (msg.Contains(":"))
                {
                    string[] temp = msg.Split(':');
                    bugly_reportException(temp[0], temp[1], stackTrace);
                }
                else 
                {
                    bugly_reportException("postException", msg, stackTrace);
                }
            }     
#endif
        }

        /// <summary>
        /// 打标签之前，需要在Bugly产品页配置中添加标签，取得标签ID后在代码中上报。
        /// </summary>
        /// <param name="sceneId"></param>
        public static void SetUsetSceneTag(int sceneId)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            buglyClass.CallStatic("setUserSceneTag", AndroidNative.currentActivity, sceneId);
#elif UNITY_IOS && !UNITY_EDITOR
            bugly_setUserSceneTag(sceneId);
#endif
        }

        /// <summary>
        /// 最多可以有9对自定义的key-value（超过则添加失败）；
        /// key限长50字节，value限长200字节，过长截断；
        /// key必须匹配正则：[a-zA-Z[0-9]]+。
        /// </summary>
        /// <param name="key"></param>
        public static void PutUserData(string key, string value)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            buglyClass.CallStatic("putUserData", AndroidNative.currentActivity, key, value);
#elif UNITY_IOS && !UNITY_EDITOR
            bugly_putUserData(key, value);
#endif
        }

        public static void SetUserId(string id)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            buglyClass.CallStatic("setUserId", id);
#elif UNITY_IOS && !UNITY_EDITOR
            bugly_setUserId(id);
#endif
        }

        public static void V(string tag, string msg)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            buglyLogClass.CallStatic("v", tag, msg);
#elif UNITY_IOS && !UNITY_EDITOR
            bugly_v(tag + "\t" + msg);
#endif
        }

        public static void D(string tag, string msg)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            buglyLogClass.CallStatic("d", tag, msg);
#elif UNITY_IOS && !UNITY_EDITOR
            bugly_d(tag + "\t" + msg);
#endif
        }

        public static void I(string tag, string msg)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            buglyLogClass.CallStatic("i", tag, msg);
#elif UNITY_IOS && !UNITY_EDITOR
            bugly_i(tag + "\t" + msg);
#endif
        }

        public static void W(string tag, string msg)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            buglyLogClass.CallStatic("w", tag, msg);
#elif UNITY_IOS && !UNITY_EDITOR
            bugly_w(tag + "\t" + msg);
#endif
        }

        public static void E(string tag, string msg)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            buglyLogClass.CallStatic("e", tag, msg);
#elif UNITY_IOS && !UNITY_EDITOR
            bugly_e(tag + "\t" + msg);
#endif
        }

#if UNITY_IOS && !UNITY_EDITOR
        [DllImport("__Internal")]
        public static extern void bugly_init(string key, bool debug, int reportLevel, bool openBlock, float blockTime);

        [DllImport("__Internal")]
        public static extern void bugly_v(string msg);

        [DllImport("__Internal")]
        public static extern void bugly_d(string msg);

        [DllImport("__Internal")]
        public static extern void bugly_i(string msg);

        [DllImport("__Internal")]
        public static extern void bugly_w(string msg);

        [DllImport("__Internal")]
        public static extern void bugly_e(string msg);

        [DllImport("__Internal")]
        public static extern void bugly_setUserId(string id);

        [DllImport("__Internal")]
        public static extern void bugly_putUserData(string key, string value);

        [DllImport("__Internal")]
        public static extern void bugly_setUserSceneTag(int tagId);

        [DllImport("__Internal")]
        public static extern void bugly_addExtraData(string msg);

        [DllImport("__Internal")]
        public static extern void bugly_reportException(string name, string reason, string info);

        [DllImport("__Internal")]
        public static extern void bugly_testCrash();
#endif
    }
}
