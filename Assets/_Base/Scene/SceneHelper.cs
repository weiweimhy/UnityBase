using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BaseFramework
{
    public class SceneHelper : Singleton<SceneHelper>
    {
        public Action<string> onSceneChanged;

        private bool isLoading = false;

        private string _lastSceneName = "";
        public string lastSceneName
        {
            get
            {
                return _lastSceneName;
            }
            private set
            {
                _lastSceneName = value;
            }
        }

        public string currentSceneName
        {
            get
            {
                return SceneManager.GetActiveScene().name;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="name">目标场景</param>
        /// <param name="minTime">最小过度时间</param>
        /// <param name="startAction">开始事件</param>
        /// <param name="endAction">结束事件</param>
        /// <param name="wait">等待其他操作</param>
        public void LoadScene(string name,
                              float minTime,
                              Action startAction,
                              Action endAction,
                              Action<Action> wait = null)
        {
            if (!isLoading)
            {
                lastSceneName = currentSceneName;

                float startTime = Time.realtimeSinceStartup;
                Log.I(this, "switch to {0} start time:{1}", name, startTime);

                if (wait != null)
                {
                    wait(() => {
                        LoadScene(name, startTime, minTime, startAction, endAction);
                    });
                }
                else
                {
                    LoadScene(name, startTime, minTime, startAction, endAction);
                }
            }
        }

        private void LoadScene(string name,
                               float startTime,
                               float minTime,
                               Action startAction,
                               Action endAction)
        {
            TaskHelper.Create<CoroutineTask>()
                                 .Do(startAction)
                                 .Wait(LoadSceneAsync(name, startTime, minTime))
                                 .Do(endAction)
                                 .Execute();
        }

        IEnumerator LoadSceneAsync(string name,
                                   float startTime,
                                   float minTime)
        {
            isLoading = true;

            AsyncOperation async = SceneManager.LoadSceneAsync(name);
            async.allowSceneActivation = false;
            while (async.progress < 0.9f)
            {
                yield return null;

            }
            while (Time.realtimeSinceStartup - startTime < minTime)
            {
                yield return null;
            }

            async.allowSceneActivation = true;

            Log.I(this, "switch to {0} end time:{1}", name, Time.realtimeSinceStartup);
            isLoading = false;
        }
    }
}
