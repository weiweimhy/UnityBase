using System;

namespace BaseFramework
{
    public class SimpleTask : Task<SimpleTask>
    {
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
