using System.Text;
using UnityEngine;

namespace BaseFramework
{
    public enum FitType
    {
        ChangeScale, ChangeSize, WidthScale, HeightScale, WidthSize, HeightSize
    }

    public enum StretchType
    {
        None, Width, Height
    }

    public class ScreenSizeManager : Singleton<ScreenSizeManager>
    {
        public const float DEFAULT_IDEAL_MIN = 1080;
        public const float DEFAULT_IDEAL_MAX = 1920;
        public const float DEFAULT_MATCH = 0.5f;

        public const float IPHONEX_MIN = 1125;
        public const float IPHONEX_MAX = 2436;
        public const float IPHONEX_INVALID_OFFSET = 140;

        private bool isInit = false;
        private ScreenModle screenModle = ScreenModle.Portrait;

        private StretchType stretchType;

        private bool isiPhoneX;

        private float idealRatio;
        private Vector2 idealSize;

        private float screenRatio;
        private Vector2 screenSize;

        private float canvasRealRatio;
        private Vector2 canvasRealSize;

        private string logString;

        public void Init(float min = DEFAULT_IDEAL_MIN, float max = DEFAULT_IDEAL_MAX, float math = DEFAULT_MATCH)
        {
            if (!isInit)
            {
                screenSize = new Vector2(Screen.width, Screen.height);
                screenRatio = screenSize.x / screenSize.y;

                screenModle = screenSize.x > screenSize.y
                                           ? ScreenModle.Landscape
                                           : ScreenModle.Portrait;

                if (screenModle == ScreenModle.Portrait)
                {
                    isiPhoneX = screenRatio.EQ(IPHONEX_MIN / IPHONEX_MAX);
                    idealSize = new Vector2(min, max);
                }
                else
                {
                    isiPhoneX = screenRatio.EQ(IPHONEX_MAX / IPHONEX_MIN);
                    idealSize = new Vector2(max, min);
                }
                idealRatio = idealSize.x / idealSize.y;

                // 计算canvas的大小
                float logWidth = Mathf.Log(screenSize.x / idealSize.x, 2f);
                float logHeight = Mathf.Log(screenSize.y / idealSize.y, 2f);
                float scaleFactor = Mathf.Pow(2f, Mathf.Lerp(logWidth, logHeight, math));
                canvasRealSize = screenSize / scaleFactor;
                // iphoneX顶部刘海，底部圆角都是无效区域
                if (isiPhoneX)
                    canvasRealSize -= screenModle == ScreenModle.Portrait
                                                  ? new Vector2(0, IPHONEX_INVALID_OFFSET)
                                                  : new Vector2(IPHONEX_INVALID_OFFSET, 0);

                canvasRealRatio = canvasRealSize.x / canvasRealSize.y;

                if (canvasRealRatio.EQ(idealRatio))
                    stretchType = StretchType.None;     // 未拉伸
                if (canvasRealRatio.GT(idealRatio))
                    stretchType = screenModle == ScreenModle.Portrait ? StretchType.Height : StretchType.Width;     // eg：iphoneX
                else 
                    stretchType = screenModle == ScreenModle.Portrait ? StretchType.Width : StretchType.Height;     //eg: ipad

                isInit = true;
            }
        }

        public float GetIdealRatio()
        {
            Init();
            return idealRatio;
        }

        public Vector2 GetIdealSize()
        {
            Init();
            return idealSize;
        }

        public float GetScreenRatio()
        {
            Init();
            return screenRatio;
        }

        public Vector2 GetScreenSize()
        {
            Init();
            return screenSize;
        }
        public float GetCanvasRealRatio()
        {
            Init();
            return canvasRealRatio;
        }

        public Vector2 GetCanvasRealSize()
        {
            Init();
            return canvasRealSize;
        }

        public ScreenModle GetScreenModle()
        {
            Init();
            return screenModle;
        }

        public StretchType GetStretchType()
        {
            Init();
            return stretchType;
        }

        public bool ShouldFix()
        {
            Init();
            return stretchType != StretchType.None;
        }

        public bool IsiPhoneX()
        {
            Init();
            return isiPhoneX;
        }

        public override string ToString()
        {
            Init();
            if(string.IsNullOrEmpty(logString))
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append("screenModle:").Append(screenModle)
                             .Append("\n idealRatio:").Append(idealRatio)
                             .Append("\n idealSize:").Append(idealSize)
                             .Append("\n screenRatio:").Append(screenRatio)
                             .Append("\n screenSize").Append(screenSize)
                             .Append("\n canvasRealRatio:").Append(canvasRealRatio)
                             .Append("\n canvasRealSize:").Append(canvasRealSize);
                logString = stringBuilder.ToString();
            }
            return logString;
        }
    }

    public enum ScreenModle
    {
        Portrait, Landscape
    }
}
