using System;

namespace BaseFramework
{
    public interface ISingleton: IDisposable
    {
        void OnSingletonInit();

        void OnSingletonDestroy();
    }
}