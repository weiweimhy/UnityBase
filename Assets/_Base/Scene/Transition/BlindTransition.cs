using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace BaseFramework
{
    public class BlindTransition : TransitionBase, ISingleton
    {
        #region singleton
        public static class SingletonHandler
        {
            static SingletonHandler()
            {
                Init();
            }

            internal static void Init()
            {
                instance = FindObjectOfType<BlindTransition>();
                if (instance == null)
                {
                    instance = new GameObject("BlindTransition").AddComponent<BlindTransition>();
                }
                else
                {
                    instance.OnSingletonInit();
                }
            }

            internal static BlindTransition instance;
        }

        public static BlindTransition instance
        {
            get
            {
                return SingletonHandler.instance;
            }
        }

        protected virtual void Awake()
        {
            if (!instance)
            {
                SingletonHandler.instance = this;

                OnSingletonInit();
            }
            else if (instance != this)  // if have multi in hierarchy, only keep one
            {
                Destroy(gameObject);
            }
        }

        public virtual void Dispose()
        {
            Destroy(gameObject);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            if (this == instance)
            {
                OnSingletonDestroy();
                SingletonHandler.instance = null;
            }
        }

        /// <summary>
        /// do something after init
        /// </summary>
        public virtual void OnSingletonInit()
        {
            DontDestroyOnLoad(gameObject);

            Init();

            gameObject.Inactive();
        }

        public virtual void OnSingletonDestroy()
        {

        }
        #endregion

        public float cellSize = 200f;
        public float rotation = 30;

        protected RectTransform container;
        protected Image[] cellList;

        private void Init()
        {
            InitCanvas();

            container = new GameObject("Container").AddComponent<RectTransform>();
            container.Parent(transform, false);
            container.SetSizeDelta(ScreenSizeManager.instance.GetCanvasSize());

            float magnitude = ScreenSizeManager.instance.GetCanvasSize().magnitude;
            int num = Mathf.CeilToInt(magnitude / cellSize);
            float size = num * cellSize;
            float startValue = size * -0.5f;
            cellList = new Image[num];
            for (int i = 0; i < num; i++)
            {
                Image cell = new GameObject("Cell" + i).AddComponent<Image>();
                cell.Color(Color.black);
                cell.transform
                    .Parent(container, false)
                    .SetSizeDelta(new Vector2(cellSize, magnitude))
                    .LocalPosition(new Vector2(startValue + (i + 0.5f) * cellSize, 0f));
                cellList[i] = cell;
            }
            container.LocalEulerAngles(Vector3.forward * rotation);
        }

        private void InitCanvas()
        {
            Canvas canvas = gameObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;

            CanvasScaler canvasScaler = gameObject.AddComponent<CanvasScaler>();
            canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            canvasScaler.referenceResolution = ScreenSizeManager.instance.GetIdealSize();
            canvasScaler.matchWidthOrHeight = 0.5f;
        }

        public override void Enter()
        {
            KillTween();

            gameObject.Active();

            tweener = DOTween.Sequence();
            int num = cellList.Length;
            for (int i = 0; i < num; ++i)
            {
                cellList[i].rectTransform.localEulerAngles = Vector3.up * 90f;
                tweener.Insert(i * 0.02f, cellList[i].rectTransform.DOLocalRotate(Vector3.zero, 0.5f, RotateMode.FastBeyond360));
            }
            tweener.OnComplete(OnEnterComplete);
        }

        public override void Exit()
        {
            KillTween();

            tweener = DOTween.Sequence();

            int num = cellList.Length;
            int num2 = 0;
            for (int i = num - 1; i >= 0; i--)
            {
                cellList[i].rectTransform.localEulerAngles = Vector3.zero;
                tweener.Insert(num2 * 0.02f, cellList[i].rectTransform.DOLocalRotate(Vector3.up * 90f, 0.5f, RotateMode.FastBeyond360));
                num2++;
            }
            tweener.OnComplete(OnExitComplete);
        }

        protected override void OnExitComplete()
        {
            OnExitCallback.InvokeGracefully(this);

            gameObject.Inactive();
        }

    }
}
