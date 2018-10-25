using System;

namespace BaseFramework
{
    public class CustomCreaterPool<T> : Pool<T> where T : IRecycleable
    {
        public CustomCreaterPool(Func<T> createFunc, int initPoolSize = 0, int maxPoolSize = int.MaxValue)
        {
            creater = new CustomCreater<T>(createFunc);
            InitPoolSize(initPoolSize, maxPoolSize);
        }
    }
}