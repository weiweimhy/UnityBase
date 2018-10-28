namespace BaseFramework
{
    public class Singleton<T> : ISingleton where T : Singleton<T>, new()
    {
        public static class SingletonHandler
        {
            /// <summary>
            /// 当一个类有静态构造函数时，它的静态成员变量不会被beforefieldinit修饰
            /// 就会确保在被引用的时候才会实例化，而不是程序启动的时候实例化
            /// </summary>
            static SingletonHandler() {
                instance = new T();
                instance.OnSingletonInit();
            }
            /// <summary>
            /// 不使用readonly是为了销毁
            /// </summary>
            internal static T instance;
        }

        public static T instance
        {
            get
            {
                return SingletonHandler.instance;
            }
        }

        public virtual void Dispose()
        {
            OnSingletonDestroy();
            SingletonHandler.instance = null;
        }

        public virtual void OnSingletonInit()
        {
        }

        public virtual void OnSingletonDestroy()
        {
            
        }
    }
}