using System;

namespace BaseFramework
{
    public interface IRecycleable : IDisposable
    {
        bool isRecycled { get; set; }

        /// <summary>
        /// Call only when created 
        /// </summary>
        void OnCreate();
        /// <summary>
        /// Call when created or reused
        /// </summary>
        void OnReset();
        /// <summary>
        /// Call when you recycle
        /// </summary>
        void OnRecycle();
    }
}