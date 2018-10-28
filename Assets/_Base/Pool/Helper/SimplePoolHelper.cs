using System;

namespace BaseFramework
{
    public static class SimplePoolHelper
    {
        private const int DEFAULT_INIT_POOL_SIZE = 10;
        private const int DEFAULT_MAX_POOL_SIZE = 10;

        public static T Create<T>(int initPoolSize = DEFAULT_INIT_POOL_SIZE,
                                  int maxPoolSize = DEFAULT_MAX_POOL_SIZE) where T : IRecycleable
        {
            return SimplePool<T>.instance.Init(initPoolSize, maxPoolSize).Create();
        }

        public static T Create<T>(Func<T> createFunc, 
                                  int initPoolSize = DEFAULT_INIT_POOL_SIZE,
                                  int maxPoolSize = DEFAULT_MAX_POOL_SIZE) where T : IRecycleable
        {
            return SimplePool<T>.instance.Init(createFunc, initPoolSize, maxPoolSize).Create();
        }

        public static bool Recycle<T>(this T self) where T : IRecycleable
        {
            return SimplePool<T>.instance.Recycle(self);
        }
    }
}