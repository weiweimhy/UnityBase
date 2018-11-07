using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace BaseFramework
{
    public class CoroutineTaskExecutor : Singleton<CoroutineTaskExecutor>, IExecutor<CoroutineTask>
    {
        private List<CoroutineTask> tasks;

        public override void OnSingletonInit()
        {
            base.OnSingletonInit();
            tasks = new List<CoroutineTask>();

            // 注册场景切换回调
            SceneManager.sceneUnloaded += OnSceneUnloaded;
        }

        public override void OnSingletonDestroy()
        {
            base.OnSingletonDestroy();
            tasks.ForEach(task => {
                task.Dispose();
            });
            tasks = null;

            SceneManager.sceneUnloaded -= OnSceneUnloaded;
        }

        public CoroutineTask Execute(CoroutineTask task)
        {
            tasks.Add(task);

            task.ExecuteInternal();
            return task;
        }

        private void OnSceneUnloaded(Scene scene)
        {
            CheckAndRecycleTask();
        }

        // 检查Task是否可用，不可用就回收
        public void CheckAndRecycleTask()
        {
            List<CoroutineTask> recycleTasks = new List<CoroutineTask>();
            tasks.ForEach(it => {
                if(CheckTask(it))
                {
                    recycleTasks.Add(it);
                }
            });
            recycleTasks.ForEach(it => RecycleTask(it));

            recycleTasks.Clear();
            recycleTasks = null;
        }

        public void CheckAndRecycleTask(CoroutineTask task)
        {
            if (CheckTask(task))
            {
                RecycleTask(task);
            }
        }

        private bool CheckTask(CoroutineTask task)
        {
            return task != null && task.ShouldRecycle() && !task.isRecycled;
        }

        private void RecycleTask(CoroutineTask task)
        {
            Log.I(this, "Recycle coroutine task [{0}]", task.name);
            task.Dispose();

            tasks.Remove(task);
        }
    }
}
