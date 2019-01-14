using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BaseFramework
{
    public class CoroutineTaskStream : SimpleRecycleItem<CoroutineTaskStream>
    {
        public enum StreamType
        {
            WAIT, ACTION
        }

        public StreamType streamType;
        public IEnumerator enumerator;
        public Action action;

        public CoroutineTaskStream Set(IEnumerator enumerator)
        {
            streamType = StreamType.WAIT;
            this.enumerator = enumerator;
            return this;
        }

        public CoroutineTaskStream Set(Action action)
        {
            streamType = StreamType.ACTION;
            this.action = action;
            return this;
        }

        public override void OnRecycle()
        {
            base.OnRecycle();
            enumerator = null;
            action = null;
        }

        public override void Dispose()
        {
            this.Recycle();
        }
    }

    public class CoroutineTask : Task<CoroutineTask>
    {
        protected MonoBehaviour behaviour;
        protected Coroutine coroutine;
        protected List<CoroutineTaskStream> streams = new List<CoroutineTaskStream>();
        protected float min;

        public CoroutineTask()
        {
        }

        public CoroutineTask(MonoBehaviour monoBehaviour)
        {
            behaviour = monoBehaviour;
        }

        public MonoBehaviour GetMonoBehaviour()
        {
            return behaviour;
        }

        public CoroutineTask SetMonoBehaviour(MonoBehaviour monoBehaviour)
        {
            behaviour = monoBehaviour;

            return this;
        }

        public CoroutineTask SetMin(float min)
        {
            this.min = min;

            return this;
        }

        public float GetMin()
        {
            return this.min;
        }

        public CoroutineTask Wait(params IEnumerator[] enumerators)
        {
            return Delay(enumerators);
        }

        public CoroutineTask Wait(params YieldInstruction[] yieldInstructions)
        {
            return Delay(yieldInstructions);
        }

        public CoroutineTask Wait(params float[] delays)
        {
            return Delay(delays);
        }

        public CoroutineTask Delay(params IEnumerator[] enumerators)
        {
            streams.Add(PoolHelper.Create<CoroutineTaskStream>().Set(CreateEnumerator(enumerators)));
            return this;
        }

        public CoroutineTask Delay(params YieldInstruction[] yieldInstructions)
        {
            streams.Add(PoolHelper.Create<CoroutineTaskStream>().Set(CreateEnumerator(yieldInstructions)));
            return this;
        }

        public CoroutineTask Delay(params float[] delays)
        {
            streams.Add(PoolHelper.Create<CoroutineTaskStream>().Set(CreateEnumerator(delays)));
            return this;
        }

        IEnumerator CreateEnumerator(params float[] delays)
        {
            if (delays.IsEmptyOrNull())
            {
                yield return null;
            }
            else
            {
                for (int i = 0; i < delays.Length; ++i)
                {
                    if (delays[i] > 0)
                        yield return new WaitForSeconds(delays[i]);
                    else
                        yield return null;
                }
            }
        }

        IEnumerator CreateEnumerator(params IEnumerator[] enumerators)
        {
            if (enumerators.IsEmptyOrNull())
            {
                yield return null;
            }
            else
            {
                for (int i = 0; i < enumerators.Length; ++i)
                {
                    if (enumerators[i] != null)
                    {
                        yield return enumerators[i];
                    }
                    else
                    {
                        yield return null;
                    }
                }
            }
        }

        IEnumerator CreateEnumerator(params YieldInstruction[] yieldInstructions)
        {
            if (yieldInstructions.IsEmptyOrNull())
            {
                yield return null;
            }
            else
            {
                for (int i = 0; i < yieldInstructions.Length; ++i)
                {
                    if (yieldInstructions[i] != null)
                    {
                        yield return yieldInstructions[i];
                    }
                    else
                    {
                        yield return null;
                    }
                }
            }
        }

        public override CoroutineTask Do(params Action[] actions)
        {
            if (actions.Length > 0)
            {
                actions.ForEach(it => Next(it));
            }
            return this;
        }

        public CoroutineTask Next(Action action)
        {
            streams.Add(PoolHelper.Create<CoroutineTaskStream>().Set(action));
            return this;
        }

        public bool ShouldRecycle()
        {
            return behaviour == null
                || (behaviour.gameObject.activeInHierarchy == false);
        }

        public override CoroutineTask Execute()
        {
            return CoroutineTaskManager.instance.Execute(this);
        }

        internal override CoroutineTask ExecuteInternal()
        {
            if (status != TaskStatus.Unexcuted)
                return this;

            Start();

            coroutine = behaviour.StartCoroutine(DoCoroutine());

            return this;
        }

        IEnumerator DoCoroutine()
        {
            float startTime = Time.realtimeSinceStartup;

            if (streams.IsEmptyOrNull())
            {
                yield return startTime == 0 ? null : new WaitForSeconds(min);
            }
            else
            {
                for (int i = 0; i < streams.Count; ++i)
                {
                    CoroutineTaskStream it = streams[i];
                    if (it.streamType == CoroutineTaskStream.StreamType.WAIT)
                    {
                        if (it.enumerator != null)
                        {
                            yield return it.enumerator;
                        }
                        else
                        {
                            yield return null;
                        }
                    }
                    else
                    {
                        it.action.InvokeGracefully();
                    }
                }
                while(Time.realtimeSinceStartup - startTime < min)
                {
                    yield return null;
                }
            }

            Finish();
        }

        public override void OnRecycle()
        {
            base.OnRecycle();
            behaviour = null;
            coroutine = null;
            streams.ForEach(it => {
                it.Dispose();
            });
            streams.Clear();
        }
    }
}
