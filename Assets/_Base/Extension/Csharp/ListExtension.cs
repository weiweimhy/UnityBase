using System;
using System.Collections.Generic;

namespace BaseFramework
{
    public static class List
    {
        public static bool IsEmptyOrNull<T>(this List<T> self)
        {
            return self.IsNull() || self.Count == 0;
        }

        public static bool IsNotEmptyAndNull<T>(this List<T> self)
        {
            return !self.IsEmptyOrNull();
        }

        public static bool Any<T>(this List<T> self)
        {
            return IsNotEmptyAndNull(self);
        }

        public static bool IsNullOrEmpty<T>(this List<T> self)
        {
            return self.IsNull() || self.Count == 0;
        }

        public static bool IsNotNullAndEmpty<T>(this List<T> self)
        {
            return !self.IsNullOrEmpty();
        }

        public static List<T> ForEachReverse<T>(this List<T> self, Action<T> action)
        {
            if (self.IsNull())
            {
                Log.W(typeof(List), "List is null");
            }
            else
            {
                for (int i = self.Count - 1; i >= 0; --i)
                    action(self[i]);
            }

            return self;
        }

        public static List<T> ForEachReverse<T>(this List<T> self, Action<int, T> action)
        {
            if (self.IsNull())
            {
                Log.W(typeof(List), "List is null");
            }
            else
            {
                for (int i = self.Count - 1; i >= 0; --i)
                    action(i, self[i]);
            }

            return self;
        }

        /// <summary>
        /// return default(T), if list is empty or null
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self"></param>
        /// <returns></returns>
        public static T GetRandomItem<T>(this List<T> self)
        {
            if (self.IsNull())
            {
                Log.W(typeof(List), "List is null");
                return default(T);
            }
            if (self.Count == 0)
            {
                Log.W(typeof(List), "List is empty");
                return default(T);
            }

            return self[UnityEngine.Random.Range(0, self.Count)];
        }

        public static T RemoveFirst<T>(this List<T> self)
        {
            if (self.IsNull())
            {
                Log.W(typeof(List), "List is null");
                return default(T);
            }
            if (self.Count == 0)
            {
                Log.W(typeof(List), "List is empty");
                return default(T);
            }

            T item = self[0];
            self.RemoveAt(0);
            return item;
        }

        public static T RemoveLast<T>(this List<T> self)
        {
            if (self.IsNull())
            {
                Log.W(typeof(List), "List is null");
                return default(T);
            }
            if (self.Count == 0)
            {
                Log.W(typeof(List), "List is empty");
                return default(T);
            }

            T item = self[self.Count - 1];
            self.RemoveAt(self.Count - 1);
            return item;
        }
    }
}
