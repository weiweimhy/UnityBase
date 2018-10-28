using System;

namespace BaseFramework
{
    public class SimplePool<T> : Pool<T>, ISingleton where T : IRecycleable
    {
        #region singletion
        public static class SingletonHandler
        {
            /// <summary>
            /// 当一个类有静态构造函数时，它的静态成员变量不会被beforefieldinit修饰
            /// 就会确保在被引用的时候才会实例化，而不是程序启动的时候实例化
            /// </summary>
            static SingletonHandler()
            {
                string a = typeof(T).Name;
                instance = new SimplePool<T>();
                instance.OnSingletonInit();
            }
            internal static SimplePool<T> instance;
        }

        public static SimplePool<T> instance
        {
            get
            {
                return SingletonHandler.instance;
            }
        }

        public virtual void OnSingletonInit()
        {

        }

        public virtual void OnSingletonDestroy()
        {
            SingletonHandler.instance = null;
        }

        public virtual void Dispose()
        {
            OnSingletonDestroy();
        }
        #endregion

        public SimplePool<T> Init(Func<T> createFunc, int initPoolSize = -1, int maxPoolSize = -1)
        {
            if (!finishInit)
            {
                creater = new SimpleCreator<T>(createFunc);
                InitPoolSize(initPoolSize, maxPoolSize);
                finishInit = true;
            }
            return this;
        }
    }
}