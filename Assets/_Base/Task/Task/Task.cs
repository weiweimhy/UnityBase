using System;

namespace BaseFramework
{
    public abstract class Task<T> : SimpleRecycleItem, ITask<T> where T : class
    {
        public string name;

        protected Action onStart = null;
        protected Action doAction = null;
        protected Action onFinish = null;
        protected Action onCancle = null;

        protected bool autoRecycle = true;

        protected TaskStatus status { get; set; }

        public T Name(string name)
        {
            this.name = name;

            return this as T;
        }

        public T OnStart(params Action[] actions)
        {
            actions.ForEach(action => onStart += action);
            return this as T;
        }

        public T Do(params Action[] actions)
        {
            actions.ForEach(action => doAction += action);
            return this as T;
        }

        public T OnFinish(params Action[] actions)
        {
            actions.ForEach(action => onFinish += action);
            return this as T;
        }

        public T OnCancle(params Action[] actions)
        {
            actions.ForEach(action => onCancle += action);
            return this as T;
        }

        public T AutoRecycle(bool auto)
        {
            autoRecycle = auto;
            return this as T;
        }

        public virtual T Execute()
        {
            return ExecuteInternal();
        }

        internal abstract T ExecuteInternal();

        public T Cancle()
        {
            if (status == TaskStatus.Excuting)
            {
                status = TaskStatus.Cancled;
                DoCancle();
            }
            else
            {
                Log.W(this,
                    "cancle failed! you can only cancle excuting task, current status is {0}", status);
            }

            return this as T;
        }

        protected virtual void DoCancle()
        {
            onCancle.InvokeGracefully();
            Dispose();
        }

        protected virtual void Start()
        {
            onStart.InvokeGracefully();
            status = TaskStatus.Excuting;
        }

        protected virtual void Do()
        {
            if (status == TaskStatus.Excuting)
                doAction.InvokeGracefully();
        }

        protected virtual void Finish()
        {
            if (status == TaskStatus.Excuting)
            {
                status = TaskStatus.Done;
                onFinish.InvokeGracefully();

                if (autoRecycle)
                    Dispose();
            }
        }

        #region IRecycleable
        public override void OnReset()
        {
            status = TaskStatus.Unexcuted;
        }

        public override void OnRecycle()
        {
            onStart = null;
            doAction = null;
            onFinish = null;
            onCancle = null;
        }
        #endregion
    }
}