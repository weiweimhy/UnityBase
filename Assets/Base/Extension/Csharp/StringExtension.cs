using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Base.Extension
{
    public static class StringExtension
    {
        public static bool IsEmptyOrNull(this string self)
        {
            return string.IsNullOrEmpty(self);
        }

        public static bool IsNotEmptyAndNull(this string self)
        {
            return !string.IsNullOrEmpty(self);
        }

        public static bool IsNullOrEmpty(this string self)
        {
            return string.IsNullOrEmpty(self);
        }

        public static bool IsNotNullAndEmpty(this string self)
        {
            return !string.IsNullOrEmpty(self);
        }

        public static string UppercaseFirst(this string self)
        {
            if (self.IsEmptyOrNull())
                return self;
            return char.ToUpper(self[0]) + self.Substring(1);
        }

        public static string LowercaseFirst(this string self)
        {
            if (self.IsEmptyOrNull())
                return self;
            return char.ToLower(self[0]) + self.Substring(1);
        }

        public static string Format(this string self, params object[] args)
        {
            if(self.IsEmptyOrNull() || args == null)
            {
                return self;
            }
            return string.Format(self, args);
        }

        public static StringBuilder Append(this string self, string appendStr)
        {
            return new StringBuilder(self).Append(appendStr);
        }

        public static StringBuilder AppendFormat(this string self, string appendFormat, params object[] args)
        {
            return new StringBuilder(self).AppendFormat(appendFormat, args);
        }

        public static string AddPrefix(this string self, string prefix)
        {
            return prefix.Append(self).ToString();
        }

        public static string AddSuffix(this string self, string suffix)
        {
            return self.Append(suffix).ToString();
        } 

        public static bool EqualsIgnoreCase(this string self, string target)
        {
            if(self.IsNull() || target.IsNull())
            {
                return false;
            }

            return self.ToLower().Equals(target.ToLower());
        }

        public static bool StartsWithIgnoreCase(this string self, string target)
        {
            if (self.IsNull() || target.IsNull())
            {
                return false;
            }

            return self.ToLower().StartsWith(target.ToLower());
        }

        public static bool EndsWithIgnoreCase(this string self, string target)
        {
            if (self.IsNull() || target.IsNull())
            {
                return false;
            }

            return self.ToLower().EndsWith(target.ToLower());
        }

        public static string ReplaceIgnoreCase(this string self, char oldValue, char newValue)
        {
            if (self.IsNull())
            {
                return self;
            }

            return self.Replace(char.ToLower(oldValue), newValue).Replace(char.ToUpper(oldValue), newValue);
        }

        public static string ReplaceIgnoreCase(this string self, String oldValue, String newValue)
        {
            if (self.IsNull() || oldValue.IsNull() || newValue.IsNull())
            {
                return self;
            }

            return self.Replace(oldValue.ToUpper(), newValue).Replace(oldValue.ToLower(), newValue);
        }

        public static int ToInt(this string self, int defaulValue = 0)
        {
            var retValue = defaulValue;
            return int.TryParse(self, out retValue) ? retValue : defaulValue;
        }

        public static float ToFloat(this string self, float defaulValue = 0)
        {
            var retValue = defaulValue;
            return float.TryParse(self, out retValue) ? retValue : defaulValue;
        }

        public static bool IsDateTime(this string self, string dateFormat)
        {
            DateTime dateVal = default(DateTime);
            return DateTime.TryParseExact(self, dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateVal);
        }

        public static DateTime ToDateTime(this string self, DateTime defaultValue = default(DateTime))
        {
            var retValue = defaultValue;
            return DateTime.TryParse(self, out retValue) ? retValue : defaultValue;
        }

        public static string Reverse(this string slef)
        {
            char[] chars = slef.ToCharArray();
            Array.Reverse(chars);
            return new String(chars);
        }

        public static bool Match(this string self, string pattern)
        {
            return Regex.IsMatch(self, pattern);
        }

        public static IEnumerable<T> SplitTo<T>(this string self, params char[] separator) where T : IConvertible
        {
            return self.Split(separator, StringSplitOptions.None).Select(s => (T)Convert.ChangeType(s, typeof(T)));
        }

        public static IEnumerable<T> SplitTo<T>(this string self, StringSplitOptions options, params char[] separator) where T : IConvertible
        {
            return self.Split(separator, options).Select(s => (T)Convert.ChangeType(s, typeof(T)));
        }

        public static string Cut(this string self, int length)
        {
            if (string.IsNullOrEmpty(self))
                return string.Empty;
            if (length < 0)
                return string.Empty;
            return self.Substring(0, Math.Min(length, self.Length));
        }

        public static string CutLast(this string self, int length)
        {
            if (string.IsNullOrEmpty(self))
                return string.Empty;
            if (length < 0)
                return string.Empty;
            return self.Substring(Math.Max(0, self.Length - length));
        }

        /// <summary>
        /// 是否存在中文字符
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static bool HasChinese(this string self)
        {
            return Regex.IsMatch(self, @"[\u4e00-\u9fa5]");
        }

        /// <summary>
        /// 删除特定字符
        /// </summary>
        /// <param name="self"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static string RemoveString(this string self, params string[] targets)
        {
            return targets.Aggregate(self, (current, t) => current.Replace(t, string.Empty));
        }
    }
}