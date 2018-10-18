using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Base.Editor.Build
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
