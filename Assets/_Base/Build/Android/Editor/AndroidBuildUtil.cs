using UnityEditor;
using UnityEditor.Callbacks;

namespace BaseFramework.Build
{
    public class AndroidBuildUtil
    {
        #region Build
        [MenuItem("Base/Build/Export Android Studio Project", false, 1)]
        public static void ExportProject()
        {
            BuildUtil.ExportTarget(BuildTarget.Android);
        }
        #endregion

        #region Android Studio project process 

        [PostProcessBuild(1)]
        public static void OnPostprocessBuild(BuildTarget buildTarget, string path)
        {
            if (buildTarget != BuildTarget.Android)
            {
                return;
            }

            // open project dir
            CommandUtil.ExecuteCommand("explorer " + path);
        }

        #endregion
    }
}
