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

        [MenuItem("AssetBundle/Build")]
        public static void BuildAssetBunlde()
        {
            string assetBundleDirectory = Application.streamingAssetsPath;
            if (!Directory.Exists(assetBundleDirectory))
                Directory.CreateDirectory(assetBundleDirectory);

            BuildTarget target = EditorUserBuildSettings.activeBuildTarget;

            BuildPipeline.BuildAssetBundles(assetBundleDirectory,
                                            BuildAssetBundleOptions.None,
                                            target);

            AssetDatabase.Refresh();
        }
    }
#endif
}
