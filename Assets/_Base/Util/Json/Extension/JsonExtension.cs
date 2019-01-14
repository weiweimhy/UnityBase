using Newtonsoft.Json.Linq;

namespace ABTest
{
    public static class JsonExtension
    {
        public static int GetInt(this JObject jsonObj, string sKey, int defValue = 0)
        {
            if (jsonObj == null)
                return defValue;

            //支持"12.3"==>12
            float fValue = GetFloat(jsonObj, sKey, defValue);
            return (int)fValue;
        }

        public static long GetLong(this JObject jsonObj, string sKey, long defValue = 0)
        {
            string strValue = GetString(jsonObj, sKey, null);
            if (strValue == null)
                return defValue;

            long lValue;
            if (!long.TryParse(strValue, out lValue))
            {
                return defValue;
            }
            return lValue;
        }

        public static string GetString(this JObject jsonObj, string sKey, string defValue = "")
        {
            if (jsonObj == null)
                return defValue;

            JToken obj = jsonObj[sKey];
            if (obj == null)
                return defValue;
            string retValue = (string)obj;
            if (retValue == null)
                return defValue;
            return retValue;
        }

        public static float GetFloat(this JObject jsonObj, string sKey, float defValue = 0)
        {
            string strValue = GetString(jsonObj, sKey, null);
            if (strValue == null)
                return defValue;

            float f2;
            if (!float.TryParse(strValue, out f2))
            {
                return defValue;
            }
            return (int)f2;
        }

        public static bool GetBool(this JObject jsonObj, string sKey, bool defValue = false)
        {
            //支持 数字 或者 true/false 

            if (jsonObj == null)
                return defValue;

            JToken obj = jsonObj[sKey];
            if (obj == null)
                return defValue;

            return (bool)obj;
        }

        public static JObject GetJObject(this JObject jsonObj, string sKey)
        {
            if (jsonObj == null)
                return null;

            return jsonObj[sKey] as JObject;
        }

        public static JArray GetJArray(this JObject jsonObj, string sKey)
        {
            if (jsonObj == null)
                return null;

            return jsonObj[sKey] as JArray;
        }
    }
}
