using System;
using System.Threading;

namespace BaseFramework
{
    public class RunableTask : Task<RunableTask>
    {
        protected Thread thread;
        protected float delayTime;

        public RunableTask Delay(float delay)
        {
            delayTime = delay;
            return this;
        }

        internal override RunableTask ExecuteInternal()
        {
            if (status != TaskStatus.Unexcuted)
                return this;

            Start();

            Action threasCallback = (() => { Finish(); });

            thread = new Thread(Run);
            thread.IsBackground = true;
            thread.Start(threasCallback);

            return this;
        }

        protected void Run(object obj)
        {
            if (delayTime > 0)
                Thread.Sleep((int)(delayTime * 1000));

            Do();
            Action callback = obj as Action;
            callback.InvokeGracefully();
        }

        protected override void DoCancle()
        {
            thread.Abort();
            base.DoCancle();
        }

        public override void OnRecycle()
        {
            base.OnRecycle();
            thread = null;
        }

        public override void Dispose()
        {
            this.Recycle();
        }
    }
}
