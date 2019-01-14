using UnityEngine;

namespace BaseFramework
{
    public class MonoSingleton<T> : MonoBehaviour, ISingleton where T : MonoSingleton<T>
    {
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
                instance = FindObjectOfType<T>() as T;
                if (instance == null)
                {
                    instance = new GameObject(typeof(T).ReadableName() + "_Singleton").AddComponent<T>();
                }
                else
                {
                    instance.OnSingletonInit();
                }
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

        protected virtual void Awake()
        {
            if (!instance)
            {
                SingletonHandler.instance = this as T;

                OnSingletonInit();
            }
            else if (instance != this)  // if have multi in hierarchy, only keep one
            {
                Destroy(gameObject);
            }
        }

        public virtual void Dispose()
        {
            Destroy(gameObject);
        }

        protected virtual void OnDestroy()
        {
            if(this == instance)
            {
                OnSingletonDestroy();
                SingletonHandler.instance = null;
            }
        }

        /// <summary>
        /// do something after init
        /// </summary>
        public virtual void OnSingletonInit()
        {
            DontDestroyOnLoad(gameObject);
        }

        public virtual void OnSingletonDestroy()
        {

        }
    }
}
