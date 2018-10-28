using System;

namespace BaseFramework
{
    public class SimpleTask : Task<SimpleTask>
    {

        protected override IExecutor<SimpleTask> InitExecutor()
        {
            // executor not use
            return null;
        }

        public override SimpleTask Execute()
        {
            return ExecuteInternal();
        }

        internal override SimpleTask ExecuteInternal()
        {
            Start();
            Do();
            Finish();

            return this;
        }

        public override void Dispose()
        {
            this.Recycle();
        }
    }
}