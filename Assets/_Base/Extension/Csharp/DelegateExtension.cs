using System;

namespace BaseFramework
{
    public static class DelegateExtension
    {
        #region Func Extension

        public static T InvokeGracefully<T>(this Func<T> self)
        {
            return null != self ? self() : default(T);
        }

        #endregion

        #region Action

        /// <summary>
        /// Call action
        /// </summary>
        /// <param name="self"></param>
        /// <returns> call succeed</returns>
        public static bool InvokeGracefully(this Action self)
        {
            if (null != self)
            {
                self();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Call action
        /// </summary>
        /// <param name="self"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static bool InvokeGracefully<T>(this Action<T> self, T t)
        {
            if (null != self)
            {
                self(t);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Call action
        /// </summary>
        /// <param name="self"></param>
        /// <returns> call succeed</returns>
        public static bool InvokeGracefully<T, K>(this Action<T, K> self, T t, K k)
        {
            if (null != self)
            {
                self(t, k);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Call delegate
        /// </summary>
        /// <param name="self"></param>
        /// <returns> call suceed </returns>
        public static bool InvokeGracefully(this Delegate self, params object[] args)
        {
            if (null != self)
            {
                self.DynamicInvoke(args);
                return true;
            }
            return false;
        }

        #endregion
    }
}