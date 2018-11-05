using System;

namespace BaseFramework
{
    public interface ITask<T> : IDisposable
    {
        T Execute();

        T Cancle();
    }
}