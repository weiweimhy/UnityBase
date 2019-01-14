using System;

namespace BaseFramework
{

    public interface IPool<T> : IDisposable where T : class
    {
        T Create();

        bool Recycle(T item);
    }
}