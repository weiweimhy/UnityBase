using System;

namespace BaseFramework
{
    public interface ITask<T> : IDisposable
    {
        TaskStatus status { get; set; }

        IExecutor<T> executor { get; }

        T Cancle();
    }
}