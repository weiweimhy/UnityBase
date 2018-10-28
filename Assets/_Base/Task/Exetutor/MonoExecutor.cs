using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace BaseFramework
{
    public class MonoExecutor : MonoSingleton<MonoExecutor>, IExecutor<CoroutineTask>
    {
        private Dictionary<string, List<CoroutineTask>> taskDic;

        public override void OnSingletonInit()
        {
            base.OnSingletonInit();
            taskDic = new Dictionary<string, List<CoroutineTask>>();

            SceneManager.sceneUnloaded += OnSceneUnloaded;
        }

        public override void OnSingletonDestroy()
        {
            base.OnSingletonDestroy();
            taskDic = null;

            SceneManager.sceneUnloaded -= OnSceneUnloaded;
        }

        public CoroutineTask Execute(CoroutineTask task)
        {
            string sceneName = SceneManager.GetActiveScene().name;
            if (taskDic.ContainsKey(sceneName))
            {
                if (taskDic[sceneName] == null)
                {
                    taskDic[sceneName] = new List<CoroutineTask>();
                }
                taskDic[sceneName].Add(task);
            }
            else
            {
                List<CoroutineTask> tasks = new List<CoroutineTask>();
                tasks.Add(task);
                taskDic.Add(sceneName, tasks);
            }

            task.ExecuteInternal();
            return task;
        }

        private void OnSceneUnloaded(Scene scene)
        {
            string sceneName = scene.name;
            if (taskDic.ContainsKey(sceneName))
            {
                taskDic[sceneName].ForEach(task => {
                    task.Dispose();
                });
            }
        }
    }
}