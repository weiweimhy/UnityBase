using UnityEngine;

namespace BaseFramework
{
    public class TaskHelper
    {
        public static T Create<T>() where T : Task<T>
        {
            return PoolHelper.Create<T>();
        }

        /// <summary>
        /// called when gameobject active false
        /// 在gameobject active false 的时候调用
        /// </summary>
        public static void CheckAndRecycle()
        {
            MonoExecutor.instance.CheckAndRecycleTask();
        }

        public static void CheckAndRecycle(CoroutineTask task)
        {
            MonoExecutor.instance.CheckAndRecycleTask(task);
        }
    }
}