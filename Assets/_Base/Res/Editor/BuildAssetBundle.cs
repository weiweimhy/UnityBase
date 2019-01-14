using System.IO;
using UnityEditor;
using UnityEngine;

namespace BaseFramework.Build
{
#if UNITY_EDITOR
    [InitializeOnLoad]
    public static class BuildAssetBundle
    {
        static BuildAssetBundle()
        {
            BuildTargetChangedHelper.changeCallback += BuildAssetBundleAuto;
        }

        public static void BuildAssetBundleAuto(BuildTarget target)
        {
            Log.I("BuildAssetBundle","Build AssetBundle Auto!");
            BuildAssetBunlde();
        }

        [MenuItem("Base/AssetBundle/Build", priority = 0)]
        public static void BuildAssetBunlde()
        {
            string assetBundleDirectory = Application.streamingAssetsPath;
            if (!Directory.Exists(assetBundleDirectory))
            {
                Directory.CreateDirectory(assetBundleDirectory);
            }

            BuildTarget target = EditorUserBuildSettings.activeBuildTarget;

            BuildPipeline.BuildAssetBundles(assetBundleDirectory,
                                            BuildAssetBundleOptions.None,
                                            target);

            AssetDatabase.Refresh();
        }

        [MenuItem("Base/AssetBundle/Clean", priority = 1)]
        public static void CleanAssetBunlde()
        {
            string assetBundleDirectory = Application.streamingAssetsPath;
            if (Directory.Exists(assetBundleDirectory))
            {
                Directory.Delete(assetBundleDirectory, true);
            }
        }

        [MenuItem("Base/AssetBundle/Rebuild", priority = 2)]
        public static void RebuildAssetBunlde()
        {
            CleanAssetBunlde();

            BuildAssetBunlde();
        }

        [MenuItem("Base/AssetBundle/Export Current Info")]
        public static void ExportCurrentMessage()
        {
            AssetBundleManager.instance.ExportCurrntMessage();
        }
    }
#endif
}
