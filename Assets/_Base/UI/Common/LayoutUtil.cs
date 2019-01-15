using UnityEngine;

namespace BaseFramework.UI
{
    public static class LayoutUtil
    {
        public enum LayoutAnchorType
        {
            TopLeft,
            TopCenter,
            TopRight,
            TopStretch,
            MiddleLeft,
            MiddleCenter,
            MiddleRight,
            MiddleStretch,
            BottomLeft,
            BottomCenter,
            BottomRight,
            BottomStretch,
            StretchLeft,
            StretchCenter,
            StretchRight,
            Stretch,
        }

        public enum LayoutPivotType
        {
            LeftTop,
            LeftCenter,
            LeftBottom,
            CenterTop,
            Center,
            CenterBottom,
            RightTop,
            RightCenter,
            RightBottom,
        }

        public static void ChangeAnchors(this RectTransform self, LayoutAnchorType layoutAnchorType)
        {
            Vector2 anchorMin = Vector2.one * 0.5f;
            Vector2 anchorMax = Vector2.one * 0.5f;
            switch (layoutAnchorType)
            {
                case LayoutAnchorType.TopLeft:
                    anchorMin = anchorMax = new Vector2(0, 1);
                    break;
                case LayoutAnchorType.TopCenter:
                    anchorMin = anchorMax = new Vector2(0.5f, 1);
                    break;
                case LayoutAnchorType.TopRight:
                    anchorMin = anchorMax = new Vector2(1, 1);
                    break;
                case LayoutAnchorType.TopStretch:
                    anchorMin = new Vector2(0, 1);
                    anchorMax = new Vector2(1, 1);
                    break;
                case LayoutAnchorType.MiddleLeft:
                    anchorMin = anchorMax = new Vector2(0, 0.5f);
                    break;
                case LayoutAnchorType.MiddleCenter:
                    anchorMin = anchorMax = new Vector2(0.5f, 0.5f);
                    break;
                case LayoutAnchorType.MiddleRight:
                    anchorMin = anchorMax = new Vector2(1, 0.5f);
                    break;
                case LayoutAnchorType.MiddleStretch:
                    anchorMin = new Vector2(0, 0.5f);
                    anchorMax = new Vector2(1, 0.5f);
                    break;
                case LayoutAnchorType.BottomLeft:
                    anchorMin = anchorMax = new Vector2(0, 0);
                    break;
                case LayoutAnchorType.BottomCenter:
                    anchorMin = anchorMax = new Vector2(0.5f, 0);
                    break;
                case LayoutAnchorType.BottomRight:
                    anchorMin = anchorMax = new Vector2(1, 0);
                    break;
                case LayoutAnchorType.BottomStretch:
                    anchorMin = new Vector2(0, 0);
                    anchorMax = new Vector2(1, 0);
                    break;
                case LayoutAnchorType.StretchLeft:
                    anchorMin = new Vector2(0, 0);
                    anchorMax = new Vector2(0, 1);
                    break;
                case LayoutAnchorType.StretchCenter:
                    anchorMin = new Vector2(0.5f, 0);
                    anchorMax = new Vector2(0.5f, 1);
                    break;
                case LayoutAnchorType.StretchRight:
                    anchorMin = new Vector2(1, 0);
                    anchorMax = new Vector2(1, 1);
                    break;
                case LayoutAnchorType.Stretch:
                    anchorMin = new Vector2(0, 1);
                    anchorMax = new Vector2(1, 0);
                    break;
            }
            ChangeAnchors(self, anchorMin, anchorMax);
        }

        public static void ChangeAnchors(this RectTransform self, Vector2 min, Vector2 max)
        {
            if (self == null)
            {
                return;
            }
            self.anchorMin = min;
            self.anchorMax = max;
        }

        public static void ChangePivot(this RectTransform self, LayoutPivotType layoutPivot)
        {
            Vector2 pivot = Vector2.one * 0.5f;
            switch (layoutPivot)
            {
                case LayoutPivotType.LeftTop:
                    pivot = new Vector2(0, 1);
                    break;
                case LayoutPivotType.LeftCenter:
                    pivot = new Vector2(0, 0.5f);
                    break;
                case LayoutPivotType.LeftBottom:
                    pivot = new Vector2(0, 0);
                    break;
                case LayoutPivotType.CenterTop:
                    pivot = new Vector2(0.5f, 1);
                    break;
                case LayoutPivotType.Center:
                    pivot = new Vector2(0.5f, 0.5f);
                    break;
                case LayoutPivotType.CenterBottom:
                    pivot = new Vector2(0.5f, 0);
                    break;
                case LayoutPivotType.RightTop:
                    pivot = new Vector2(1, 1);
                    break;
                case LayoutPivotType.RightCenter:
                    pivot = new Vector2(1, 0.5f);
                    break;
                case LayoutPivotType.RightBottom:
                    pivot = new Vector2(1, 0);
                    break;
            }
            ChangePivot(self, pivot);
        }

        public static void ChangePivot(this RectTransform self, Vector2 pivot)
        {
            if (self == null)
            {
                return;
            }
            self.pivot = pivot;
        }

        private static LayoutPivotType GetAdjustPivotFromAnchor(LayoutAnchorType layoutAnchorType)
        {
            switch (layoutAnchorType)
            {
                case LayoutAnchorType.TopLeft:
                    return LayoutPivotType.LeftTop;

                case LayoutAnchorType.TopCenter:
                case LayoutAnchorType.TopStretch:
                    return LayoutPivotType.CenterTop;

                case LayoutAnchorType.TopRight:
                    return LayoutPivotType.RightTop;

                case LayoutAnchorType.MiddleLeft:
                case LayoutAnchorType.StretchLeft:
                    return LayoutPivotType.LeftCenter;

                case LayoutAnchorType.MiddleStretch:
                case LayoutAnchorType.MiddleCenter:
                case LayoutAnchorType.StretchCenter:
                case LayoutAnchorType.Stretch:
                    return LayoutPivotType.Center;

                case LayoutAnchorType.MiddleRight:
                case LayoutAnchorType.StretchRight:
                    return LayoutPivotType.RightCenter;

                case LayoutAnchorType.BottomLeft:
                    return LayoutPivotType.LeftBottom;

                case LayoutAnchorType.BottomCenter:
                case LayoutAnchorType.BottomStretch:
                    return LayoutPivotType.CenterBottom;

                case LayoutAnchorType.BottomRight:
                    return LayoutPivotType.RightBottom;
            }
            return LayoutPivotType.Center;
        }

        public static void ChangeAnchorAndPivot(this RectTransform self, LayoutAnchorType layoutAnchorType)
        {
            ChangeAnchorAndPivot(self, layoutAnchorType, GetAdjustPivotFromAnchor(layoutAnchorType));
        }

        public static void ChangeAnchorAndPivot(this RectTransform self, LayoutAnchorType layoutAnchorType, LayoutPivotType layoutPivotType)
        {
            ChangeAnchors(self, layoutAnchorType);
            ChangePivot(self, layoutPivotType);
        }
    }
}
