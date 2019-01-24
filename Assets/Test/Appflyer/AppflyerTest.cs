using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BaseFramework.ThirdPlugin.Appsflyer;
using BaseFramework.ThirdPlugin.Bugly;

public class AppflyerTest : MonoBehaviour
{
    public const string appKey = "qNsTSFixbaPufCv5sQ6yJV";
    public const string iosAppId = "1342112505";

    private const string androidID = "995eda8a4d";
    private const string iOSID = "99b94062b0";

    private void InitBugly()
    {
#if UNITY_ANDROID
        BuglyUtil.Init(androidID, true);
#elif UNITY_IOS
        BuglyUtil.Init(iOSID, true);
#endif
    }

    private void Awake()
    {
        InitBugly();
    }

    public void Init()
    {
        AppsFlyerUtil.Init(appKey, iosAppId, true);
    }

    public void TrackAppLaunch()
    {
        AppsFlyerUtil.TrackAppLaunch();
    }

    public void LogEvent()
    {
        Dictionary<string, string> dic = new Dictionary<string, string>();
        dic.Add("test_event_key", "test_event_value");
        AppsFlyerUtil.LogEvent("test_event_name", dic);
    }

    public void GetUID()
    {
        Debug.Log("[AppflyerTest] uid: " + AppsFlyerUtil.GetUID());
    }

    public void ValidateAndTrackIAP()
    {
        Dictionary<string, string> dic = new Dictionary<string, string>();
        dic.Add("test_event_key", "test_event_value");
        AppsFlyerUtil.ValidateAndTrackIAP("test_product", "test_signature",
        "purchaseData", "0.99", "currency", dic);
    }
}
