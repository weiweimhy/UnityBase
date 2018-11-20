using System;
using System.Reflection;

namespace BaseFramework
{
    public static class ReflectionExtension
    {
        public static bool HasAttribute(this PropertyInfo self, Type attributeType, bool inherit)
        {
            if (null == self)
                return false;
            return Attribute.IsDefined(self, attributeType);
        }

        public static bool HasAttribute(this FieldInfo self, Type attributeType, bool inherit)
        {
            if (null == self)
                return false;
            return Attribute.IsDefined(self, attributeType);
        }

        public static bool HasAttribute(this Type self, Type attributeType, bool inherit)
        {
            if (null == self)
                return false;
            return Attribute.IsDefined(self, attributeType);
        }

        public static bool HasAttribute(this MethodInfo self, Type attributeType, bool inherit)
        {
            if (null == self)
                return false;
            return Attribute.IsDefined(self, attributeType);
        }

        public static T GetFirstAttribute<T>(this MethodInfo self, bool inherit) where T : Attribute
        {
            T[] attributes = GetAttributes<T>(self, inherit);
            if (attributes.Any())
                return attributes[0];
            return null;
        }

        public static T[] GetAttributes<T>(this MethodInfo self, bool inherit) where T : Attribute
        {
            if (null != self)
                return (T[])self.GetCustomAttributes(typeof(T), inherit);
            return null;
        }

        public static T GetFirstAttribute<T>(this FieldInfo self, bool inherit) where T : Attribute
        {
            T[] attributes = GetAttributes<T>(self, inherit);
            if (attributes.Any())
                return attributes[0];
            return null;
        }

        public static T[] GetAttributes<T>(this FieldInfo self, bool inherit) where T : Attribute
        {
            if (null != self)
                return (T[])self.GetCustomAttributes(typeof(T), inherit);
            return null;
        }

        public static T GetFirstAttribute<T>(this PropertyInfo self, bool inherit) where T : Attribute
        {
            T[] attributes = GetAttributes<T>(self, inherit);
            if (attributes.Any())
                return attributes[0];
            return null;
        }

        public static T[] GetAttributes<T>(this PropertyInfo self, bool inherit) where T : Attribute
        {
            if (null != self)
                return (T[])self.GetCustomAttributes(typeof(T), inherit);
            return null;
        }

        public static T GetFirstAttribute<T>(this Type self, bool inherit) where T : Attribute
        {
            T[] attributes = GetAttributes<T>(self, inherit);
            if (attributes.Any())
                return attributes[0];
            return null;
        }

        public static T[] GetAttributes<T>(this Type self, bool inherit) where T : Attribute
        {
            if (null != self)
                return (T[])self.GetCustomAttributes(typeof(T), inherit);
            return null;
        }
    }
}
