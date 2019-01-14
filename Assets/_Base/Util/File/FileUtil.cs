namespace BaseFramework
{
    public class FileUtil
    {
        /// <summary>
        /// 获取短名字，包括后缀
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetShortName(string path)
        {
            int startIndex = path.LastIndexOf("/");
            if (startIndex > 0)
            {
                path = path.Substring(startIndex + 1, path.Length - startIndex - 1);
            }

            return path;
        }

        /// <summary>
        /// 获取短名字，不包括后缀
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetShortNameNoSuffix(string path)
        {
            path = GetShortName(path);

            int endIndex = path.LastIndexOf(".");
            if (endIndex > 0)
            {
                path = path.Substring(0, endIndex);
            }

            return path;
        }

        /// <summary>
        /// 获取后缀，不包含'.'，eg:"xxx.mp3" return mp3
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetSuffix(string path)
        {
            if (string.IsNullOrEmpty(path))
                return "";

            int startIndex = path.LastIndexOf(".");
            if (startIndex > 0)
            {
                path = path.Substring(startIndex + 1, path.Length - startIndex - 1);
            }

            return path;
        }
    }
}
