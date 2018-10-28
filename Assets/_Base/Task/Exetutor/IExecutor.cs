namespace BaseFramework
{
    public interface IExecutor<T>
    {
        T Execute(T task);
    }
}