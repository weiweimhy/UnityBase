using System;

namespace BaseFramework
{
    public class CustomCreater<T> : ICreater<T>
    {
        protected Func<T> createFunc;

        public CustomCreater(Func<T> createFunc)
        {
            this.createFunc = createFunc;
        }

        public T Create()
        {
            return createFunc();
        }
    }
}