using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

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
    
        private static void Init(string appkey, AppsflyerCallbackListener callback,
                                bool isDebug, bool setUerId = true, bool enableIMEI = false)
        {
            utilClass.CallStatic("init", AndroidNative.currentActivity, appkey, callback, isDebug, setUerId, enableIMEI);
        }
#endif

        public static void LogEvent(string eventName, Dictionary<string, string> eventValues)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            utilClass.CallStatic("logEvent", AndroidNative.currentActivity, eventName, AndroidNative.ConvertDictToHashMap(eventValues));
#elif UNITY_IOS && !UNITY_EDITOR
            af_logEvent(eventName, JsonUtility.ToJson(eventValues));
#endif
        }

        public static string GetUID()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            return utilClass.CallStatic<string>("getUID", AndroidNative.currentActivity);
#elif UNITY_IOS && !UNITY_EDITOR
            return af_getUID();
#endif
            return "";
        }

        public static void ValidateAndTrackIAP(string publicKey, string signature, string purchaseData,
                                               string price, string currency, Dictionary<string, string> additionalParameters)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            utilClass.CallStatic("validateAndTrackIAP", AndroidNative.currentActivity, publicKey, signature,
                                 purchaseData, price, currency, AndroidNative.ConvertDictToHashMap(additionalParameters));
#elif UNITY_IOS  && !UNITY_EDITOR
            af_validateAndTrackInAppPurchase(publicKey, price, currency, signature, JsonUtility.ToJson(additionalParameters));
#endif
        }




        public static void Init(string key, string appid, bool debug, bool setUerId = true, bool enableIMEI = false)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            AppsflyerCallbackListener listener = new AppsflyerCallbackListener();
            Init(key, listener, debug, setUerId, enableIMEI);
#elif UNITY_IOS && !UNITY_EDITOR
            string delgateObjectName = "AppsflyerCallbackListenerIOS";
            GameObject go = new GameObject(delgateObjectName);
            go.AddComponent<AppsflyerCallbackListenerIOS>();
            go.hideFlags = HideFlags.HideInHierarchy;
            Object.DontDestroyOnLoad(go);
            af_init(key, appid, delgateObjectName, debug);
#endif
        }

        public static void TrackAppLaunch()
        {
#if UNITY_ANDROID && !UNITY_EDITOR

#elif UNITY_IOS  && !UNITY_EDITOR
            af_trackAppLaunch();
#endif
        }

#if UNITY_IOS  && !UNITY_EDITOR
        public class AppsflyerCallbackListenerIOS : MonoBehaviour
        {
            public virtual void onConversionDataReceived(string installDataJson)
            {

            }

            public virtual void validateAndTrackInAppPurchaseSuccess(string responseJson)
            {

            }

            public virtual void validateAndTrackInAppPurchaseFailure(string errJson)
            {

            }
        }

        [DllImport("__Internal")]
        public static extern void af_init(string key, string appid, string delgateObjectName, bool debug);

        [DllImport("__Internal")]
        public static extern void af_trackAppLaunch();

        [DllImport("__Internal")]
        public static extern void af_logEvent(string eventName, string json);

        [DllImport("__Internal")]
        public static extern string af_getUID();

        [DllImport("__Internal")]
        public static extern void af_validateAndTrackInAppPurchase(string productId, string price, string currency, string tranactionId, string json);
#endif
    }
}
