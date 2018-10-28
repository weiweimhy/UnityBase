using System;
using System.Collections;
using UnityEngine;

namespace BaseFramework
{
    public class CoroutineTask : Task<CoroutineTask>
    {
        protected MonoBehaviour behaviour;
        protected IEnumerator enumerator;
        protected Coroutine coroutine;

        protected override IExecutor<CoroutineTask> InitExecutor()
        {
            return MonoExecutor.instance;
        }

        public CoroutineTask MonoBehaviour(MonoBehaviour monoBehaviour)
        {
            behaviour = monoBehaviour;

            return this;
        }

        public CoroutineTask Delay(IEnumerator enumerator)
        {
            this.enumerator = CreateEnumerator(enumerator);
            return this;
        }

        public CoroutineTask Delay(YieldInstruction yieldInstruction)
        {
            enumerator = CreateEnumerator(enumerator);
            return this;
        }

        public CoroutineTask Delay(float delay)
        {
            enumerator = CreateEnumerator(new WaitForSeconds(delay));
            return this;
        }

        IEnumerator CreateEnumerator(IEnumerator enumerator)
        {
            yield return enumerator;

            Do();
            Finish();
        }

        IEnumerator CreateEnumerator(YieldInstruction yieldInstruction)
        {
            yield return yieldInstruction;

            Do();
            Finish();
        }

        internal override CoroutineTask ExecuteInternal()
        {
            if (behaviour == null || enumerator == null)
            {
                throw new NullReferenceException("behaviour or enumerator is null, you should init first!");
            }

            if (status != TaskStatus.Unexcuted)
                return this;

            Start();

            coroutine = behaviour.StartCoroutine(enumerator);

            return this;
        }

        public override void Dispose()
        {
            this.Recycle();
        }

        public override void OnRecycle()
        {
            base.OnRecycle();
            behaviour = null;
            enumerator = null;
            coroutine = null;
        }
    }
}