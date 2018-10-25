using System;
using System.Collections.Generic;

namespace BaseFramework
{
    public abstract class Pool<T> : IPool<T> where T : IRecycleable
    {
        protected Stack<T> cacheStack;
        protected ICreater<T> creater;
        protected int maxPoolSize;

        protected void InitPoolSize(int initPoolSize, int maxPoolSize)
        {
            this.maxPoolSize = maxPoolSize;
            if (initPoolSize <= 0)
            {
                cacheStack = new Stack<T>();
            }
            else
            {
                cacheStack = new Stack<T>(initPoolSize);
            }
        }

        public T Create()
        {
            if(cacheStack.Count > 0)
            {
                T reuseItem = cacheStack.Pop();
                reuseItem.IsRecycled = false;
                reuseItem.OnReset();
                return reuseItem;
            }

            T newItem = creater.Create();
            newItem.IsRecycled = false;
            newItem.OnCreate();
            newItem.OnReset();
            return newItem;
        }

        public bool Recycle(T item)
        {
            if (item == null || item.IsRecycled)
                return false;

            if (cacheStack.Count >= maxPoolSize)
            {
                item.OnRecycle();
                return false;
            }

            item.IsRecycled = true;
            item.OnRecycle();
            cacheStack.Push(item);

            return true;
        }
    }
}