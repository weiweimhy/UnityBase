using System.Runtime.InteropServices;

namespace BaseFramework
{
    public class Log2iOS
    {
#if UNITY_IOS && !UNITY_EDITOR
    [DllImport ("__Internal")]
    public static extern void v(string msg);

    [DllImport ("__Internal")]
    public static extern void d(string msg);

    [DllImport ("__Internal")]
    public static extern void i(string msg);

    [DllImport ("__Internal")]
    public static extern void w(string msg);

    [DllImport ("__Internal")]
    public static extern void e(string msg);
#endif
    }
}
