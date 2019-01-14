namespace BaseFramework
{
#if UNITY_EDITOR
    using UnityEditor;

    public class ClassExtensionExample
    {
        [MenuItem("Base/Example/Extension/Class")]
        public static void RunExample()
        {
            var example = new object();

            if(example.IsNull()) // example == null
            {

            }
            else if(example.IsNotNull()) // example != null
            {

            }

            Log.I(typeof(ClassExtensionExample), "this example output nothing, you should read source code");
        }
    }

#endif

    public static class ClassExtension
    {

        public static bool IsNull<T>(this T self) where T:class
        {
            return null == self;
        }

        public static bool IsNotNull<T>(this T self) where T:class
        {
            return null != self;
        }

    }
}
