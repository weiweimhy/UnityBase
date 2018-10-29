using UnityEngine;
using UnityEngine.SceneManagement;

namespace BaseFramework.Test
{
    public class TaskTest : MonoBehaviour
    {
        // Use this for initialization
        void Start()
        {
            // 场景切换以后停止执行
            #region
            this.Delay(1, () => {
                Log.I(this, "延迟1s执行");
            });

            this.ExcuteTask(5, () => {
                Log.I(this, "延迟5s执行");
            });

            CoroutineTask coroutineTask = TaskHelper.Create<CoroutineTask>()
                .MonoBehaviour(this)
                .Delay(6)
                .OnStart(() => { Log.I(this, "coroutineTask [Onstart], {0}", Time.realtimeSinceStartup); })
                .Do(() => { Log.I(this, "coroutineTask [Do], {0}", Time.realtimeSinceStartup); })
                .OnFinish(() => { Log.I(this, "coroutineTask [onfinish] {0}", Time.realtimeSinceStartup); })
                .OnCancle(() => { Log.I(this, "coroutineTask [oncancle] {0}", Time.realtimeSinceStartup); })
                .Execute();

            // coroutineTask.Cancle();
            #endregion

            // 场景切换以后会继续执行
            RunableTask threadTask = TaskHelper.Create<RunableTask>()
                .Delay(10)
                .OnStart(() => { Log.I(this, "threadTask [Onstart], {0}", System.DateTime.Now); })
                .Do(() => { Log.I(this, "threadTask [Do], {0}", System.DateTime.Now); }) // Do将在其他线程运行, 使用Time.realtimeSinceStartup会Crash
                .OnFinish(() => { Log.I(this, "threadTask [OnFinish] {0}", System.DateTime.Now); })
                .OnCancle(() => { Log.I(this, "threadTask [OnCancle] {0}", System.DateTime.Now); })
                .Execute();

            // threadTask.Cancle();
        }

        public void SwitchScene(string name)
        {
            SceneManager.LoadScene(name);
        }
    }
}