using UnityEngine;

namespace BaseFramework
{
    public class ScreenFit : MonoBehaviour
    {
        [Range(0,1)]
        public float matchWidthOrHeight = 0.5f;
        [Tooltip("selected matchWidthOrHeight will not use")]
        public bool auto = true;
        public FitType fitType;

        private RectTransform rectTransform;

        private void Awake()
        {
            rectTransform = transform as RectTransform;
            Fit();
        }

        private void Fit()
        {           
            if (ScreenSizeManager.instance.ShouldFix())
            {
                if (auto)
                {
                    if (ScreenSizeManager.instance.GetStretchType() == StretchType.Height)
                        // �߶����죬���ݸ߶�����
                        matchWidthOrHeight = 1;
                    else
                        // ������죬���ݿ������
                        matchWidthOrHeight = 0;
                }

                Vector2 scaleVector = ScreenSizeManager.instance.GetCanvasRealSize() / ScreenSizeManager.instance.GetIdealSize();
                float logWidth = Mathf.Log(scaleVector.x, 2f);
                float logHeight = Mathf.Log(scaleVector.y, 2f);
                float scale = Mathf.Pow(2f, Mathf.Lerp(logWidth, logHeight, matchWidthOrHeight));

                switch (fitType)
                {
                    case FitType.ChangeScale:
                        transform.LocalScale(transform.localScale * scale);
                        break;
                    case FitType.WidthScale:
                        transform.LocalScaleX(transform.localScale.x * scale);
                        break;
                    case FitType.HeightScale:
                        transform.LocalScaleY(transform.localScale.y * scale);
                        break;
                    case FitType.ChangeSize:
                        rectTransform.sizeDelta = rectTransform.sizeDelta * scale;
                        break;
                    case FitType.WidthSize:
                        rectTransform.sizeDelta = rectTransform.sizeDelta.NewX(rectTransform.sizeDelta.x * scale);
                        break;
                    case FitType.HeightSize:
                        rectTransform.sizeDelta = rectTransform.sizeDelta.NewY(rectTransform.sizeDelta.y * scale);
                        break;
                }
            }
        }
    }
}
