using UnityEngine;

namespace BaseFramework
{
    public abstract class MonoRecycleItem<T> : MonoBehaviour, IRecycleable where T : class, IRecycleable
    {

        public bool isRecycled { get; set; }

        public virtual void OnCreate()
        {

        }

        public virtual void OnReset()
        {
        }

        public virtual void OnRecycle()
        {
            this.Inactive();
        }

        public virtual void Dispose()
        {
            (this as T).Recycle();
        }
    }
}
