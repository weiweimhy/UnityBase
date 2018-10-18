using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Base.Editor.Build
{
    public class BuildUtil
    {
        private static string TAG = "[BuildUtil] ";

        public static void ExportTarget(BuildTarget buildTarget)
        {
            if (EditorUserBuildSettings.activeBuildTarget != buildTarget)
            {
                BuildTargetChangedHelper.changeCallback += Build2Target;
                BuildTargetChangedHelper.Switch2BuildTarget(buildTarget);
            } 
            else
            {
                ExportProject(buildTarget);
            }
        }

        private static void Build2Target(BuildTarget buildTarget)
        {
            ExportProject(buildTarget);
            BuildTargetChangedHelper.changeCallback -= Build2Target;
        }

        private static bool ExportProject(BuildTarget buildTarget)
        {
            string exportPath = null;
            if (buildTarget == BuildTarget.iOS)
                exportPath = BuildProjectSetting.instance.iOSExportPath;
            else if(buildTarget == BuildTarget.Android)
                exportPath = BuildProjectSetting.instance.androidExportPath;

            if (string.IsNullOrEmpty(exportPath))
            {
                Debug.LogWarning(TAG + "Build error:export path is empty!");
                return false;
            }

            if (Directory.Exists(exportPath))
                Directory.Delete(exportPath, true);

            if(buildTarget == BuildTarget.Android)
            {
                EditorUserBuildSettings.androidBuildSystem = AndroidBuildSystem.Gradle;
                // Export Android Project for use with Android Studio/Gradle.
                EditorUserBuildSettings.exportAsGoogleAndroidProject = true;
                PlayerSettings.Android.bundleVersionCode = BuildProjectSetting.instance.androidVersionCode;
                PlayerSettings.bundleVersion = BuildProjectSetting.instance.androidVersionName;
            }
            else if(buildTarget == BuildTarget.iOS)
            {
                PlayerSettings.iOS.buildNumber = BuildProjectSetting.instance.iOSBuildCode.ToString();
                PlayerSettings.bundleVersion = BuildProjectSetting.instance.iOSVersionName;
            }

            string[] scenes = FindEnabledEditorScenes();
            // BuildOptions.AcceptExternalModificationsToPlayer： Used when building Xcode (iOS) or Eclipse (Android) projects.
            string report = BuildPipeline.BuildPlayer(scenes, exportPath, buildTarget,
                                            BuildOptions.AcceptExternalModificationsToPlayer);

            if (!string.IsNullOrEmpty(report))
            {
                Debug.LogError("build message: " + report);
                return false;
            }
            else
            {
                Debug.Log("Build Project: Succeed");
            }

            return true;
        }

        private static string[] FindEnabledEditorScenes()
        {
            List<string> EditorScenes = new List<string>();
            foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
            {
                if (!scene.enabled)
                    continue;
                EditorScenes.Add(scene.path);
            }
            return EditorScenes.ToArray();
        }
    }
}
