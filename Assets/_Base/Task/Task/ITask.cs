using System;

namespace BaseFramework
{
    public interface ITask<T> : IDisposable
    {
        TaskStatus status { get; set; }

        T Execute();

        T Cancle();
    }
}