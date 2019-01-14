using System;
using System.Linq.Expressions;

namespace BaseFramework
{
    public class SimpleCreator<T> : ICreator<T>
    {
        private Func<T> createFunc;

        public SimpleCreator()
        {
            // throw exception whild call createFunc if no public Constructor 
            createFunc = () => Activator.CreateInstance<T>();
        }

        public SimpleCreator(Func<T> func)
        {
            createFunc = func;
        }

        public T Create()
        {
            return createFunc();
        }
    }
}