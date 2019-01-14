#if UNITY_ANDROID && UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;

namespace BaseFramework.Build
{
    [InitializeOnLoad]
    public static class AndroidGradleUtil
    {
        private static string sourcePath = "/_Base/Build/Android/Res/mainTemplate.gradle";
        private static readonly string targetPath = Path.Combine(Application.dataPath, "Plugins/Android/mainTemplate.gradle");

        static AndroidGradleUtil()
        {
            BuildUtil.onPreProcessBuild += CopyGradle;        
        }

        private static void CopyGradle()
        {
            Log.I("AndroidGradleUtil", "Copy gradle file");

            FileInfo fileInfo = new FileInfo(targetPath);
            if (fileInfo.Exists)
            {
                fileInfo.Delete();
            }
            if (!fileInfo.Directory.Exists)
            {
                fileInfo.Directory.Create();
            }
            File.Copy(Application.dataPath + sourcePath, targetPath);
            AssetDatabase.Refresh();
        }

        [MenuItem("Base/Build/Editor Android Gradle")]
        public static void EditorGradle()
        {
            Selection.activeObject = AssetDatabase.LoadAssetAtPath<Object>("Assets" + sourcePath);
        }
    }
}
#endif