using UnityEngine;

namespace BaseFramework
{
    public class MonoPool<T> : Pool<T>, ISingleton where T : class, IRecycleable
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
            internal static void Init() {
                _instance = new MonoPool<T>();
                _instance.OnSingletonInit();
            }

            private static MonoPool<T> _instance;
            internal static MonoPool<T> instance
            {
                get
                {
                    if(_instance == null)
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

        public static MonoPool<T> instance
        {
            get
            {
                return SingletonHandler.instance;
            }
        }

        private GameObject poolRootObj;

        public virtual void OnSingletonInit()
        {
            poolRootObj = new GameObject().Name(string.Format("{0}_Singleton", GetType().ReadableName()));
            poolRootObj.transform.SetParent(MonoPoolManager.instance.transform);
            poolRootObj.Inactive();
        }

        public GameObject GetGameObject()
        {
            return poolRootObj;
        }

        public MonoPool<T> GameObjectName(string name)
        {
            poolRootObj.Name(name);
            return this;
        }

        public MonoPool<T> Parent(Transform parent)
        {
            poolRootObj.transform.Parent(parent);
            return this;
        }

        public virtual void OnSingletonDestroy()
        {
            poolRootObj.Destroy();
            poolRootObj = null;
            SingletonHandler.instance = null;
        }

        public override void Dispose()
        {
            base.Dispose();
            OnSingletonDestroy();
        }
        #endregion

        protected override void OnItemRecycle(T item)
        {
            base.OnItemRecycle(item);

            (item as MonoBehaviour).transform.SetParent(poolRootObj.transform, false);
        }

        protected override bool CheckUseful(T t)
        {
            return t as MonoBehaviour != null;
        }
    }
}
