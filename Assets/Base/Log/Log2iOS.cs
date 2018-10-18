using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class Log2iOS {

#if UNITY_IOS
    [DllImport ("__Internal")]
    public static extern void V(string msg);

    [DllImport ("__Internal")]
    public static extern void D(string msg);

    [DllImport ("__Internal")]
    public static extern void I(string msg);

    [DllImport ("__Internal")]
    public static extern void W(string msg);

    [DllImport ("__Internal")]
    public static extern void E(string msg);
#endif
}
