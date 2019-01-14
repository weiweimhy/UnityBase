using UnityEngine;

namespace BaseFramework
{
    public static class FloatExtension
    {
        /// <summary>
        /// 等于
        /// </summary>
        /// <param name="self"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static bool EQ(this float self, float target)
        {
            return Mathf.Abs(self - target) < Mathf.Epsilon;
        }

        /// <summary>
        /// 不等
        /// </summary>
        /// <param name="self"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static bool NE(this float self, float target)
        {
            return !EQ(self, target);
        }

        /// <summary>
        /// 小于
        /// </summary>
        /// <param name="self"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static bool LT(this float self, float target)
        {
            return !EQ(self, target) && self < target;
        }

        /// <summary>
        /// 大于
        /// </summary>
        /// <param name="self"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static bool GT(this float self, float target)
        {
            return !EQ(self, target) && self > target;
        }

        /// <summary>
        /// 小于等于
        /// </summary>
        /// <param name="self"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static bool LE(this float self, float target)
        {
            return !GT(self, target);
        }

        /// <summary>
        /// 大于等于
        /// </summary>
        /// <param name="self"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static bool GE(this float self, float target)
        {
            return !LT(self, target);
        }
    }
}
