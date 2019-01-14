using System;

namespace BaseFramework
{
    public class TaskHelper
    {
        public static T Create<T>() where T : Task<T>
        {
            return PoolHelper.Create<T>();
        }

        public static T Create<T>(Func<T> createFunc) where T : Task<T>
        {
            return PoolHelper.Create<T>(createFunc);
        }

        /// <summary>
        /// called when gameobject active false
        /// 在gameobject active false 的时候调用
        /// </summary>
        public static void CheckAndRecycle()
        {
            CoroutineTaskManager.instance.CheckAndRecycleTask();
        }

        public static void CheckAndRecycle(CoroutineTask task)
        {
            CoroutineTaskManager.instance.CheckAndRecycleTask(task);
        }
    }
}
