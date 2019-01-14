using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace BaseFramework
{
    public class PlayerPrefsUtil
    {
        public static void SetStrings(string key, List<string> datas)
        {
            if (datas == null || datas.Count == 0)
            {
                PlayerPrefs.SetString(key, "");
                return;
            }

            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < datas.Count; ++i)
            {
                stringBuilder.Append(datas[i]);
                if (i != datas.Count - 1)
                    stringBuilder.Append(",");
            }

            PlayerPrefs.SetString(key, stringBuilder.ToString());
        }

        public static List<string> GetStrings(string key)
        {
            List<string> result = new List<string>();

            string data = PlayerPrefs.GetString(key);

            if (string.IsNullOrEmpty(data))
            {
                return result;
            }

            string[] datas = data.Split(',');
            result = new List<string>(datas);

            return result;
        }

        public static void SetInts(string key, List<int> datas, char split = ',')
        {
            if (datas.IsEmptyOrNull())
            {
                PlayerPrefs.SetString(key, "");
                return;
            }

            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < datas.Count; ++i)
            {
                stringBuilder.Append(datas[i]);
                if (i != datas.Count - 1)
                    stringBuilder.Append(split);
            }

            PlayerPrefs.SetString(key, stringBuilder.ToString());
        }

        public static List<int> GetInts(string key, char split = ',')
        {
            List<int> result = new List<int>();

            string data = PlayerPrefs.GetString(key);

            if (string.IsNullOrEmpty(data))
            {
                return result;
            }

            string[] datas = data.Split(split);
            foreach (string item in datas)
            {
                try
                {
                    int v = int.Parse(item.Trim());
                    result.Add(v);
                }
                catch (System.Exception e)
                {
                    Log.E("PlayerPrefsUtil", item + " can not parse to int, error:" + e.Message);
                }
            }

            return result;
        }

        public static void SetLong(string key, long value)
        {
            PlayerPrefs.SetString(key, value.ToString());
        }

        public static long GetLong(string key)
        {
            return System.Convert.ToInt64(PlayerPrefs.GetString(key));
        }
    }
}
