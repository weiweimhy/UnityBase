using System;

namespace BaseFramework
{
    public class SimplePool<T> : Pool<T>, ISingleton where T : class, IRecycleable
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
                Init();
            }
            internal static void Init()
            {
                _instance = new SimplePool<T>();
                _instance.OnSingletonInit();
            }

            private static SimplePool<T> _instance;
            internal static SimplePool<T> instance
            {
                get
                {
                    if (_instance == null)
                    {
                        Init();
                    }
                    return _instance;
                }
                set
                {
                    _instance = value;
                }
            }
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

        public override void Dispose()
        {
            base.Dispose();
            OnSingletonDestroy();
        }
        #endregion
    }
}
