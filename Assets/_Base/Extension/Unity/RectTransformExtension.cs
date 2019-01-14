using UnityEngine;

namespace BaseFramework
{
    public static class RectTransformExtension
    {
        /// <summary>
        /// self的中心相对target锚点的便宜坐标
        /// </summary>
        /// <param name="self"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static Vector2 RelativePosition(this RectTransform self, RectTransform target)
        {
            // throw NullException will self or target is null
            return RectTransformUtility.CalculateRelativeRectTransformBounds(target, self).center;
        }

        public static Vector2 GetWorldSize(this RectTransform self)
        {
            Vector3[] corners = new Vector3[4];
            self.GetWorldCorners(corners);
            Vector2 size = corners[2] - corners[0];
            size.x = Mathf.Abs(size.x);
            size.y = Mathf.Abs(size.y);
            return size;
        }

        public static RectTransform AnchorPosX(this RectTransform self, float anchorPosX)
        {
            var anchorPos = self.anchoredPosition;
            anchorPos.x = anchorPosX;
            self.anchoredPosition = anchorPos;
            return self;
        }

        public static RectTransform AnchorPosY(this RectTransform self, float anchorPosY)
        {
            var anchorPos = self.anchoredPosition;
            anchorPos.y = anchorPosY;
            self.anchoredPosition = anchorPos;
            return self;
        }

        public static RectTransform SetSizeWidth(this RectTransform self, float sizeWidth)
        {
            var sizeDelta = self.sizeDelta;
            sizeDelta.x = sizeWidth;
            self.sizeDelta = sizeDelta;
            return self;
        }

        public static RectTransform SetSizeHeight(this RectTransform self, float sizeHeight)
        {
            var sizeDelta = self.sizeDelta;
            sizeDelta.y = sizeHeight;
            self.sizeDelta = sizeDelta;
            return self;
        }

        public static RectTransform SetSize(this RectTransform self, Vector2 size)
        {
            var sizeDelta = self.sizeDelta;
            sizeDelta.x = size.x;
            sizeDelta.y = size.y;
            self.sizeDelta = sizeDelta;
            return self;
        }
    }
}