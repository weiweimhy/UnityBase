using System;
using System.Collections.Generic;

namespace BaseFramework
{
    public static class DictionaryExtension 
    {
        public static bool IsEmptyOrNull<K, V>(this Dictionary<K, V> self)
        {
            return self.IsNull() || self.Count == 0;
        }

        public static bool IsNotEmptyAndNull<K, V>(this Dictionary<K, V> self)
        {
            return !self.IsEmptyOrNull();
        }

        public static bool IsNullOrEmpty<K, V>(this Dictionary<K, V> self)
        {
            return self.IsNull() || self.Count == 0;
        }

        public static bool IsNotNullAndEmpty<K, V>(this Dictionary<K, V> self)
        {
            return !self.IsNullOrEmpty();
        }

        public static Dictionary<K, V> ForEach<K, V>(this Dictionary<K, V> self, Action<K, V> action)
        {
            if (self.IsNull())
            {
                Log.W(typeof(List), "Dictionary is null");
            }
            else
            {
                foreach(KeyValuePair<K, V> item in self)
                {
                    action(item.Key, item.Value);
                }
            }

            return self;
        }

        public static Dictionary<K, V> AddRange<K, V>(this Dictionary<K, V> self,
                                                      Dictionary<K, V> sourceDic,
                                                      bool isOverride = false)
        {
            if (self.IsNull())
            {
                Log.W(typeof(List), "Dictionary is null");
            } 
            else
            {
                if(sourceDic.IsEmptyOrNull())
                {
                    Log.W(typeof(List), "Dictionary is null");
                    return self;
                }

                foreach (KeyValuePair<K, V> item in sourceDic)
                {
                    if(self.ContainsKey(item.Key))
                    {
                        if(isOverride)
                        {
                            self[item.Key] = item.Value;
                        }
                    }
                    else
                    {
                        self.Add(item.Key, item.Value);
                    }
                }
            }

            return self;
        }
    }
}
