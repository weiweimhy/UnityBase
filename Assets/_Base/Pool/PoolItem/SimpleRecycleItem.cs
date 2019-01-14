namespace BaseFramework
{
    public abstract class SimpleRecycleItem<T> : IRecycleable where T : class, IRecycleable
    {
        public bool isRecycled { get; set; }

        public virtual void OnCreate()
        {
        }

        public virtual void OnRecycle()
        {
        }

        public virtual void OnReset()
        {
        }

        public virtual void Dispose()
        {
            (this as T).Recycle();
        }
    }
}
