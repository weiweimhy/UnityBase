using System;
using Base.LogUtil;

namespace Base.Extension
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

        public static bool IsNullOrEmpty<T>(this T[] self)
        {
            return self.IsNull() || self.Length == 0;
        }

        public static bool IsNotNullAndEmpty<T>(this T[] self)
        {
            return !self.IsNullOrEmpty();
        }

        public static T[] ForEach<T>(this T[] self, Action<T> action)
        {
            if (self.IsNull())
            {
                Log.W(typeof(ArrayExtension), "Array is null!");
            }
            else
            {
                Array.ForEach(self, action);
            }

            return self;
        }
    }
}
