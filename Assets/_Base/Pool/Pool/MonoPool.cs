using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace BaseFramework
{
    public class MonoPoolManager : MonoSingleton<MonoPoolManager>
    {

        public override void OnSingletonInit()
        {
            base.OnSingletonInit();

            instance.Inactive();
        }

        public void Register(UnityAction<Scene> action)
        {
            SceneManager.sceneUnloaded += action;
        }

        public void UnRegister(UnityAction<Scene> action)
        {
            SceneManager.sceneUnloaded -= action;
        }
    }

    public enum MonoPoolType
    {
        Lasting, // 持久的，在场景切换不销毁，需要手动销毁
        AutoDispose //场景销毁的时候自动销毁
    }

    public class MonoPool<T> : Pool<T>, ISingleton where T : class, IRecycleable
    {
        private GameObject poolRootObj;
        private MonoPoolType poolType = MonoPoolType.Lasting;

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
                instance = new MonoPool<T>();
                instance.OnSingletonInit();
            }

            internal static MonoPool<T> instance;
        }

        public static MonoPool<T> instance
        {
            get
            {
                return SingletonHandler.instance;
            }
        }

        public virtual void OnSingletonInit()
        {
            poolRootObj = new GameObject();
            poolRootObj
                .Inactive()
                .Name(string.Format("{0}_Singleton", GetType().ReadableName()))
                .transform.SetParent(MonoPoolManager.instance.transform, false);
        }

        public virtual void OnSingletonDestroy()
        {
            poolRootObj.Destroy();
            poolRootObj = null;
            SingletonHandler.instance = null;
        }
        #endregion

        public MonoPool<T> SetType(MonoPoolType monoPoolType)
        {
            if (monoPoolType != poolType)
            {
                poolType = monoPoolType;
                if (poolType == MonoPoolType.Lasting)
                {
                    MonoPoolManager.instance.UnRegister(OnSceneUnloaded);
                }
                else
                {
                    MonoPoolManager.instance.Register(OnSceneUnloaded);
                }
            }

            return this;
        }

        private void OnSceneUnloaded(Scene scene)
        {
            MonoPoolManager.instance.UnRegister(OnSceneUnloaded);

            Dispose();
        }

        protected override void OnItemRecycle(T item)
        {
            base.OnItemRecycle(item);

            (item as MonoBehaviour).transform
                                   .Inactive()
                                   .Parent(poolRootObj.transform, false);
        }

        protected override bool CheckUseful(T t)
        {
            return t as MonoBehaviour != null;
        }

        public override void Dispose()
        {
            base.Dispose();
            OnSingletonDestroy();
        }
    }
}
