using System;

namespace BaseFramework
{
    public static class ArrayExtension
    {
        public static bool IsEmptyOrNull<T>(this T[] self)
        {
            return self.IsNull() || self.Length == 0;
        }

        public static bool IsNotEmptyAndNull<T>(this T[] self)
        {
            return !self.IsEmptyOrNull();
        }

        public static bool Any<T>(this T[] self)
        {
            return IsNotEmptyAndNull(self);
        }

        public static bool IsNullOrEmpty<T>(this T[] self)
        {
            return self.IsNull() || self.Length == 0;
        }

        public static bool IsNotNullAndEmpty<T>(this T[] self)
        {
            return !self.IsNullOrEmpty();
        }

        public static T[] ForEach<T>(this T[] self, Action<int, T> action)
        {
            if (self.IsNull())
            {
                Log.W(typeof(ArrayExtension), "Array is null!");
            }
            else
            {
                for(int i = 0;i < self.Length; ++i)
                {
                    action(i, self[i]);
                }
            }

            return self;
        }
    }
}
