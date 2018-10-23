using System;

namespace BaseFramework
{
    public static class LogTag
    {
        public static string GetLogTag(this object obj, string format = "[{0}]", params object[] args)
        {
            if (obj.IsNull())
            {
                return GetLogTag("null", format, args);
            }

            Type type = obj.GetType();
            if (type.IsTypeof<string>())
            {
                return format.Format(obj);
            }
            else if (type.IsSimple())
            {
                return GetLogTag(obj.ToString(), format, args);
            }
            return GetLogTag(type.Name, format, args);
        }
    }
}