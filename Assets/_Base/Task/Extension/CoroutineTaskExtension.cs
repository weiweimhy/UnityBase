using System;
using System.Collections;
using UnityEngine;

namespace BaseFramework
{
    public static class CoroutineTaskExtension
    {
        public static CoroutineTask ExcuteTask(this MonoBehaviour self,
                                               IEnumerator enumerator,
                                               params Action[] argActions)
        {
            return TaskHelper.Create<CoroutineTask>()
                             .SetMonoBehaviour(self)
                             .Delay(enumerator)
                             .Do(argActions)
                             .Execute();
        }

        public static CoroutineTask ExcuteTask(this MonoBehaviour self,
                                               YieldInstruction yieldInstruction,
                                               params Action[] argActions)
        {
            return TaskHelper.Create<CoroutineTask>()
                             .SetMonoBehaviour(self)
                             .Delay(yieldInstruction)
                             .Do(argActions)
                             .Execute();
        }

        public static void Delay(this MonoBehaviour self,
                                 float seconds,
                                 params Action[] argActions)
        {
            ExcuteTask(self, seconds > 0 ? new WaitForSeconds(seconds) : null, argActions).Name(self.name + "-delay-" + seconds);
        }
    }
}
