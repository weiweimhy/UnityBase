using System;
using System.Collections.Generic;

namespace BaseFramework
{
    public static class List
    {
        public static bool IsEmptyOrNull<T>(this IList<T> self)
        {
            return self.IsNull() || self.Count == 0;
        }

        public static bool IsNotEmptyAndNull<T>(this IList<T> self)
        {
            return !self.IsEmptyOrNull();
        }

        public static bool Any<T>(this IList<T> self)
        {
            return IsNotEmptyAndNull(self);
        }

        public static bool IsNullOrEmpty<T>(this IList<T> self)
        {
            return self.IsNull() || self.Count == 0;
        }

        public static bool IsNotNullAndEmpty<T>(this IList<T> self)
        {
            return !self.IsNullOrEmpty();
        }

        public static IList<T> ForEach<T>(this IList<T> self, Action<int, T> action)
        {
            if (self.IsNull())
            {
                Log.W(typeof(List), "List is null");
            }
            else
            {
                for (int i = 0; i < self.Count; ++i)
                    action(i, self[i]);
            }

            return self;
        }

        public static IList<T> ForEachReverse<T>(this IList<T> self, Action<T> action)
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

        public static IList<T> ForEachReverse<T>(this IList<T> self, Action<int, T> action)
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
        public static T GetRandomItem<T>(this IList<T> self)
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

        public static T RemoveFirst<T>(this IList<T> self)
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

        public static T RemoveLast<T>(this IList<T> self)
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

        public static IList<T> AddAdll<T>(this IList<T> self, IList<T> source)
        {
            if (self.IsNotNull() && source.Any())
            {
                foreach (T item in source)
                {
                    self.Add(item);
                }
            }

            return self;
        }

        public static void Shuffle<T>(this IList<T> self, int count = -1)
        {
            if (count == -1)
                count = self.Count;
            Random random = new Random();
            while (count > 0)
            {
                int one = random.Next(0, self.Count);
                int two = random.Next(0, self.Count);
                T value = self[one];
                self[one] = self[two];
                self[two] = value;
                count--;
            }
        }
    }
}
