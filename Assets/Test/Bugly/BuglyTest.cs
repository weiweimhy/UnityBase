using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

using BaseFramework.ThirdPlugin.Bugly;

public class BuglyTest : MonoBehaviour
{
    private const string androidID = "995eda8a4d";
    private const string iOSID = "99b94062b0";

    public void Init()
    {
#if UNITY_ANDROID
        BuglyUtil.Init(androidID, true);
#elif UNITY_IOS
        BuglyUtil.Init(iOSID, true);
#endif
    }

    public void AddExtraData()
    {
        BuglyUtil.AddExtraData("test_extra_data");
    }

    public void PostException()
    {
        BuglyUtil.PostException("test_post_exception", "test_post_exception_stacktrace");
    }

    public void SetUseSceneTag()
    {
        BuglyUtil.SetUsetSceneTag(-1);
    }

    public void PutUserData()
    {
        BuglyUtil.PutUserData("testuserkey", "testuservalue");
    }

    public void SetUserId()
    {
        BuglyUtil.SetUserId("test_user_id");
    }

    public void Log_V()
    {
        BuglyUtil.V("[test]", "log_v_test");
    }

    public void Log_D()
    {
        BuglyUtil.D("[test]", "log_d_test");
    }

    public void Log_I()
    {
        BuglyUtil.I("[test]", "log_i_test");
    }

    public void Log_W()
    {
        BuglyUtil.W("[test]", "log_w_test");
    }

    public void Log_E()
    {
        BuglyUtil.E("[test]", "log_e_test");
    }

    public void Test_IOS_Crash()
    {
#if UNITY_IOS && !UNITY_EDITOR
        BuglyUtil.bugly_testCrash();
#endif
    }

    public void Test_CS_Exception1()
    {
        int zero = 0;
        int i = 10 / zero;
    }

    public void Test_CS_Exception2()
    {
        Test_CS_Exception2_2();
    }

    public void Test_CS_Exception2_2()
    {
        Test_CS_Exception2_3();
    }

        public void Test_CS_Exception2_3()
    {
        Test_CS_Exception2_4();
    }

        public void Test_CS_Exception2_4()
    {
        Test_CS_Exception2_5();
    }

    public void Test_CS_Exception2_5()
    {
        Debug.Log("[BuglyTest] indexoutofrangeException before1");
        for(int i = 0; i < 10000;++i){
            int j = i+i;
        }
        Debug.Log("[BuglyTest] indexoutofrangeException before2");
        int[] arr = new int[2];
        int c = arr[10];
    }

    public void Test_CS_Exception3()
    {
        Debug.Log("[BuglyTest] nullreferenceException before1");
        for(int i = 0; i < 10000;++i){
            int j = i+i;
        }
        Debug.Log("[BuglyTest] nullreferenceException before2");
        GameObject go = null;
        go.name = "test";
    }
}
