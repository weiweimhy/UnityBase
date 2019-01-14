using System.Collections.Generic;
using UnityEngine;

namespace BaseFramework.ThirdPlugin.Appsflyer
{
    public class AppsFlyerUtil
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        public class AppsflyerCallbackListener : AndroidJavaProxy
        {
            public AppsflyerCallbackListener() : base("com.android4unity.util.appsflyer.AppsflyerCallbackListener") { }

            public virtual void onInstallConversionDataLoaded(string msg)
            {
                Log.I(this, "onInstallConversionDataLoaded1: " + msg);
            }

            public virtual void onInstallConversionFailure(string msg)
            {

            }

            public virtual void onAppOpenAttribution(string msg)
            {

            }

            public virtual void onAttributionFailure(string msg)
            {

            }

            public virtual void onValidateInApp()
            {

            }

            public virtual void onValidateInAppFailure(string msg)
            {

            }
        }

        private static AndroidJavaClass _utilClass;
        private static AndroidJavaClass utilClass
        {
            get
            {
                if (_utilClass == null)
                {
                    _utilClass = new AndroidJavaClass("com.android4unity.util.appsflyer.Util");
                }
                return _utilClass;
            }
        }

        public static void Init(string appkey, AppsflyerCallbackListener callback,
                                bool isDebug, bool setUerId = true, bool enableIMEI = false)
        {

            utilClass.CallStatic("init", AndroidNative.currentActivity, appkey, callback, isDebug, setUerId, enableIMEI);
        }

        public static void LogEvent(string eventName, Dictionary<string, string> eventValues)
        {
            utilClass.CallStatic("logEvent", AndroidNative.currentActivity, eventName, AndroidNative.ConvertDictToHashMap(eventValues));
        }

        public static string GetUID()
        {
            return utilClass.CallStatic<string>("getUID", AndroidNative.currentActivity);
        }

        public static void ValidateAndTrackIAP(string publicKey, string signature, string purchaseData,
                                               string price, string currency, Dictionary<string, string> additionalParameters)
        {
            utilClass.CallStatic("validateAndTrackIAP", AndroidNative.currentActivity, publicKey, signature,
                                 purchaseData, price, currency, AndroidNative.ConvertDictToHashMap(additionalParameters));
        }
#endif
    }
}
