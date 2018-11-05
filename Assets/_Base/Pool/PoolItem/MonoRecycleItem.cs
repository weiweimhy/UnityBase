using UnityEngine;

namespace BaseFramework
{
    public abstract class MonoRecycleItem: MonoBehaviour, IRecycleable
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
        }

        public abstract void Dispose();
    }
}
