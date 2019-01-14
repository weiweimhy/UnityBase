using UnityEngine;

namespace BaseFramework
{

    public class ScreenFitiPhoneX : MonoBehaviour
    {
#if UNITY_IOS || UNITY_EDITOR
        public enum FitType
        {
            Top, Bottom, Full
        }

        public FitType fitType = FitType.Full;
        private RectTransform rectTransform;

        private void Awake()
        {
            if (ScreenSizeManager.instance.IsiPhoneX())
            {
                rectTransform = transform as RectTransform;

                float iPhoneXOffset = ScreenSizeManager.IPHONEX_INVALID_OFFSET / 2;

                switch (fitType)
                {
                    case FitType.Full:
                        rectTransform.offsetMax -= new Vector2(0, iPhoneXOffset);
                        rectTransform.offsetMin += new Vector2(0, iPhoneXOffset);
                        break;
                    case FitType.Top:
                        rectTransform.offsetMax -= new Vector2(0, iPhoneXOffset);
                        break;
                    case FitType.Bottom:
                        rectTransform.offsetMin += new Vector2(0, iPhoneXOffset);
                        break;
                }
            }
        }
#endif
    }
}
