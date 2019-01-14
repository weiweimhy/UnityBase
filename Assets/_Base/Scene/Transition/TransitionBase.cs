using System;
using DG.Tweening;
using UnityEngine;

namespace BaseFramework
{
    public abstract class TransitionBase : MonoBehaviour
    {
        public Action<TransitionBase> OnEnterCallback;
        public Action<TransitionBase> OnExitCallback;
        protected Sequence tweener;

        protected virtual void OnDestroy()
        {
            KillTween();
            OnEnterCallback = null;
            OnExitCallback = null;
        }

        protected void KillTween()
        {
            if (tweener != null)
            {
                tweener.Kill(false);
                tweener = null;
            }
        }

        public abstract void Enter();

        public abstract void Exit();

        protected virtual void OnEnterComplete()
        {
            OnEnterCallback.InvokeGracefully(this);
        }

        protected virtual void OnExitComplete()
        {
        }
    }
}
