namespace BaseFramework
{

    public static class objectExtension
    {

        public static T As<T>(this object self) where T : class
        {
            return self as T;
        }
    }
}
