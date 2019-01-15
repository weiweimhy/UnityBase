using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BaseFramework.UI
{
    [RequireComponent(typeof(Image))]
    public abstract class Dialog : MonoBehaviour
    {
        public enum DialogStatus
        {
            None,
            Init,
            Opening,
            Opened,
            Closing,
            Disable,
            Closed
        }

        public DialogStatus dialogStatus;
        public bool ignoreRaycaster = false;
        public bool dontDestroyOnLoad = false;
        public bool destroyOnClose = true;
        public bool closeByBack = true;
        public bool closeByClickBlank = false;
        public bool playOpenSound = true;
        public bool playCloseSound = true;
        public bool playOpenAnimation = true;
        public bool playCloseAnimation = true;
        public float openAnimationTime = 0.5f;
        public float closeAnimationTime = 0.25f;
        public Color bgColor = new Color(0, 0, 0, 0.7f);

        public string dialogName { get; private set; }

        protected Transform contentTransform;
        protected Image bgImage;
        protected GraphicRaycaster graphicRaycaster;

        protected Action<Dialog> onOpened;
        private Action<Dialog> onClose;
        private Action<Dialog> onDisable;
        private Action<Dialog> onDestroy;

        protected virtual void Awake()
        {
            SetCamera();

            graphicRaycaster = GetComponent<GraphicRaycaster>();
            bgImage = GetComponent<Image>();
            contentTransform = transform.Find("ContentPanel");
            if (contentTransform == null)
            {
                contentTransform = transform.Find("FitPanel/ContentPanel");
            }

            transform.SetAsLastSibling();

            bgImage.color = bgColor;

            SetIgnoreRaycaster(ignoreRaycaster);

            if (dontDestroyOnLoad)
            {
                DontDestroyOnLoad(gameObject);
            }

            dialogStatus = DialogStatus.Init;
        }

        protected virtual void OnEnable()
        {
            dialogStatus = DialogStatus.Opening;

            if (playOpenSound)
            {
                PlayOpenSound();
            }

            if (playOpenAnimation && contentTransform != null)
            {
                PlayOpenAnimation();
            }
            else
            {
                onOpened.InvokeGracefully(this);

                dialogStatus = DialogStatus.Opened;
            }
        }

        private void Update()
        {
            if (closeByBack && Input.GetKeyDown(KeyCode.Escape))
            {
                Close();
            }
        }

        protected virtual void OnDisable()
        {
            dialogStatus = DialogStatus.Disable;

            onDisable.InvokeGracefully(this);
        }

        protected virtual void OnDestroy()
        {
            dialogStatus = DialogStatus.None;

            onDestroy.InvokeGracefully(this);

            onOpened = null;
            onClose = null;
            onDisable = null;
            onDestroy = null;
        }

        protected abstract void SetCamera();

        public virtual Dialog SetDialogName(string name)
        {
            this.dialogName = name;

            return this;
        }

        public virtual Dialog SetIgnoreRaycaster(bool ignoreRaycaster)
        {
            this.ignoreRaycaster = ignoreRaycaster;
            graphicRaycaster.enabled = !ignoreRaycaster;

            return this;
        }

        protected virtual void PlayOpenSound()
        {

        }

        protected virtual void PlayCloseSound()
        {

        }

        protected virtual void PlayOpenAnimation()
        {
            contentTransform.localScale = Vector3.zero;
            bgImage.color = Vector4.zero;

            DOTween.Sequence()
                .SetUpdate(true)
                .Append(bgImage.DOFade(bgColor.a, openAnimationTime))
                .Insert(0, contentTransform.DOScale(Vector3.one, openAnimationTime))
                .OnComplete(() => {
                    onOpened.InvokeGracefully(this);

                    dialogStatus = DialogStatus.Opened;
                });
        }

        protected virtual void PlayCloseAnimation()
        {
            DOTween.Sequence()
                   .SetUpdate(true)
                   .Append(bgImage.DOFade(0f, closeAnimationTime))
                   .Insert(0, contentTransform.DOScale(Vector3.zero, closeAnimationTime))
                   .OnComplete(() => {
                       DoClose();
                   });
        }

        public virtual void OnPointerDown(PointerEventData eventData)
        {
            if (closeByClickBlank && eventData.pointerEnter == gameObject)
            {
                Close();
            }
        }

        public virtual Dialog Open()
        {
            if (!IsOpened())
            {
                gameObject.SetActive(true);
            }

            return this;
        }

        public virtual bool IsOpened()
        {
            if (this == null || gameObject == null)
            {
                return false;
            }
            return dialogStatus == DialogStatus.Opening && gameObject.activeSelf;
        }

        public virtual void Close()
        {
            if (dialogStatus == DialogStatus.Opened)
            {
                onClose.InvokeGracefully(this);

                if (playCloseSound)
                {
                    PlayCloseSound();
                }

                dialogStatus = DialogStatus.Closing;
                if (playCloseAnimation)
                {
                    PlayCloseAnimation();
                }
                else
                {
                    DoClose();
                }
            }
        }

        private void DoClose()
        {
            if (destroyOnClose)
            {
                gameObject.Destroy();
            }
            else
            {
                gameObject.Inactive();
            }

            dialogStatus = DialogStatus.Closed;
        }

        public Dialog OnOpend(Action<Dialog> action)
        {
            onOpened += action;

            return this;
        }

        public Dialog OnClose(Action<Dialog> action)
        {
            onClose += action;

            return this;
        }

        public Dialog OnDisable(Action<Dialog> action)
        {
            onDisable += action;

            return this;
        }

        public Dialog OnDestroy(Action<Dialog> action)
        {
            onDestroy += action;

            return this;
        }
    }
}
