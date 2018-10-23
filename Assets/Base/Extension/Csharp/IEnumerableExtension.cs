using System;
using System.Collections.Generic;
using Base.LogUtil;

namespace Base.Extension
{
    public static class IEnumerableExtension
    {
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> self, Action<T> action)
        {
            if (self.IsNull())
            {
                Log.W(typeof(ArrayExtension), "IEnumerable is null!");
            }
            else
            {
                foreach (var item in self)
                {
                    action(item);
                }
            }

            return self;
        }
    }
}
