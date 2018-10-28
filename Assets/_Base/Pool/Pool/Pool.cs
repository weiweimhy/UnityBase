using System;
using System.Collections.Generic;

namespace BaseFramework
{
    public abstract class Pool<T> : IPool<T> where T : IRecycleable
    {
        protected Stack<T> cacheStack;
        protected ICreator<T> creater;
        protected int maxPoolSize;

        protected bool finishInit;

        public Pool<T> Init(int initPoolSize = -1, int maxPoolSize = -1)
        {
            if (!finishInit)
            {
                creater = new SimpleCreator<T>();
                InitPoolSize(initPoolSize, maxPoolSize);
                finishInit = true;
            }
            return this;
        }

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
            if (!finishInit)
                Init();

            if(cacheStack.Count > 0)
            {
                T reuseItem = cacheStack.Pop();
                reuseItem.isRecycled = false;
                reuseItem.OnReset();
                return reuseItem;
            }

            T newItem = creater.Create();
            newItem.isRecycled = false;
            newItem.OnCreate();
            newItem.OnReset();
            return newItem;
        }

        public bool Recycle(T item)
        {
            if (item == null || item.isRecycled)
                return false;

            if (maxPoolSize > 0 && cacheStack.Count >= maxPoolSize)
            {
                item.OnRecycle();
                return false;
            }

            item.isRecycled = true;
            item.OnRecycle();
            cacheStack.Push(item);

            return true;
        }
    }
}