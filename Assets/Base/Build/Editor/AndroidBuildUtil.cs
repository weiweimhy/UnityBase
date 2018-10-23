using UnityEditor;

namespace BaseFramework.Build
{
    public class AndroidBuildUtil
    {
        #region Build
        [MenuItem("Build/Export Android Studio Project", false, 1)]
        public static void ExportXCodeProject()
        {
            BuildUtil.ExportTarget(BuildTarget.Android);
        }
        #endregion
    }
}
