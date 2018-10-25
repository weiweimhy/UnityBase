namespace BaseFramework
{
    public interface IPool<T>
    {
        T Create();

        bool Recycle(T item);
    }
}