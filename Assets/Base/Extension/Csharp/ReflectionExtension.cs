using System;
using System.Reflection;

namespace BaseFramework
{
    public static class ReflectionExtension
    {

        /// <summary>
        /// 通过反射方式调用函数
        /// </summary>
        /// <param name="self"></param>
        /// <param name="methodName">方法名</param>
        /// <param name="args">参数</param>
        /// <returns></returns>
        public static object InvokeByReflect(this object self, string methodName, params object[] args)
        {
            if (self.IsNull())
                return null;

            MethodInfo methodInfo = null;

            if (args != null && args.Length > 0)
            {
                Type[] types = new Type[args.Length];
                for (int i = 0; i < args.Length; ++i)
                {
                    types[i] = args[i].GetType();
                }
                methodInfo = self.GetType().GetMethod(methodName, types);
            }
            else
            {
                methodInfo = self.GetType().GetMethod(methodName);
            }

            return methodInfo == null ? null : methodInfo.Invoke(self, args);
        }

        /// <summary>
        /// 通过反射方式获取域值
        /// </summary>
        /// <param name="self"></param>
        /// <param name="fieldName">域名</param>
        /// <returns></returns>
        public static object GetFieldByReflect(this object self, string fieldName)
        {
            FieldInfo fieldInfo = null;
            if(null != self)
            {
                fieldInfo = self.GetType().GetField(fieldName);
            }
            return fieldInfo == null ? null : fieldInfo.GetValue(self);
        }

        /// <summary>
        /// 通过反射方式获取属性
        /// </summary>
        /// <param name="self"></param>
        /// <param name="fieldName">属性名</param>
        /// <returns></returns>
        public static object GetPropertyByReflect(this object self, string propertyName, object[] index = null)
        {
            PropertyInfo propertyInfo = null;
            if (null != self)
            {
                propertyInfo = self.GetType().GetProperty(propertyName);
            }
            return propertyInfo == null ? null : propertyInfo.GetValue(self, index);
        }

        /// <summary>
        /// 拥有特性
        /// </summary>
        /// <returns></returns>
        public static bool HasAttribute(this PropertyInfo self, Type attributeType, bool inherit)
        {
            if (null == self)
                return false;
            return self.GetCustomAttributes(attributeType, inherit).Length > 0;
        }

        /// <summary>
        /// 拥有特性
        /// </summary>
        /// <returns></returns>
        public static bool HasAttribute(this FieldInfo self, Type attributeType, bool inherit)
        {
            if (null == self)
                return false;
            return self.GetCustomAttributes(attributeType, inherit).Length > 0;
        }

        /// <summary>
        /// 拥有特性
        /// </summary>
        /// <returns></returns>
        public static bool HasAttribute(this Type self, Type attributeType, bool inherit)
        {
            if (null == self)
                return false;
            return self.GetCustomAttributes(attributeType, inherit).Length > 0;
        }

        /// <summary>
        /// 拥有特性
        /// </summary>
        /// <returns></returns>
        public static bool HasAttribute(this MethodInfo self, Type attributeType, bool inherit)
        {
            if (null == self)
                return false;
            return self.GetCustomAttributes(attributeType, inherit).Length > 0;
        }


        /// <summary>
        /// 获取第一个特性
        /// </summary>
        public static T GetFirstAttribute<T>(this MethodInfo self, bool inherit) where T : Attribute
        {
            if(null != self)
            {
                T[] attrs = (T[]) self.GetCustomAttributes(typeof(T), inherit);
                if (attrs != null && attrs.Length > 0)
                    return attrs[0];
            }
            return null;
        }

        /// <summary>
        /// 获取第一个特性
        /// </summary>
        public static T GetFirstAttribute<T>(this FieldInfo field, bool inherit) where T : Attribute
        {
            T[] attrs = (T[]) field.GetCustomAttributes(typeof(T), inherit);
            if (attrs != null && attrs.Length > 0)
                return attrs[0];
            return null;
        }

        /// <summary>
        /// 获取第一个特性
        /// </summary>
        public static T GetFirstAttribute<T>(this PropertyInfo self, bool inherit) where T : Attribute
        {
            if(null != self)
            {
                T[] attrs = (T[]) self.GetCustomAttributes(typeof(T), inherit);
                if (attrs != null && attrs.Length > 0)
                    return attrs[0];
            }
            return null;
        }

        /// <summary>
        /// 获取第一个特性
        /// </summary>
        public static T GetFirstAttribute<T>(this Type self, bool inherit) where T : Attribute
        {
            if(null != self)
            {
                T[] attrs = (T[]) self.GetCustomAttributes(typeof(T), inherit);
                if (attrs != null && attrs.Length > 0)
                    return attrs[0];
            }
            return null;
        }
    }
}