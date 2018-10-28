namespace BaseFramework
{
    public class SimpleRecycleItem : IRecycleable
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
    }
}
