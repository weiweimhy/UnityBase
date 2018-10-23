using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.iOS.Xcode.Custom;
using UnityEngine;

namespace Base.Editor.Build
{
    public class BuildProjectSetting : ScriptableObject
    {
        [Header("iOS project setting")]
        [Space(5)]
        [Header("version:")]
        public string iOSVersionName;
        public int iOSBuildCode;
        [Space(5)]
        [Header("exportPath:")]
        public string iOSExportPath;
        [Header("Sign team id")]
        public string signTeamID;
        [Space(5)]
        [Header("capabilitys:")]
        public List<iOSCapabilityType> capabilitys;
        [Header("frameworks:")]
        public List<string> frameworks;
        [Header("libraries:")]
        public List<string> libraries;
        [Header("properties:")]
        public List<KeyValueItem> properties;
        [Header("plists:")]
        public List<KeyValueItem> plists;
        [Header("Add files, path is relative to Assets/")]
		public List<Object> files;

        [Space(50)]
        [Header("version:")]
        public string androidVersionName;
        public int androidVersionCode;
        [Header("Android project setting")]
        public string androidExportPath;

        #region BuildProjectSetting
        private static readonly string BUILD_PROJECT_SETTING_PATH = "Assets/Base/Build/ProjectSetting";
        private static readonly string BUILD_PROJECT_SETTING_NAME = "BuildProjectSetting";

        private static readonly string BUILD_PROJECT_SETTING_FILE_PATH = Path.Combine(BUILD_PROJECT_SETTING_PATH, BUILD_PROJECT_SETTING_NAME + ".asset");

        #region singleton

        private static BuildProjectSetting _instance;

        public static BuildProjectSetting instance
        {
            get
            {
                if (!_instance)
                {
                    _instance = FindBuildProjectSetting();
                }

                if (!_instance)
                {
                    _instance = CreateSettingForce();
                }

                return _instance;
            }
        }

        #endregion

        #region Creaete Build Project Setting

        [MenuItem("Build/Edit Build Project Setting", false, 0)]
        public static void CreateSetting()
        {
            if (instance != null)
            {
                Selection.activeObject = instance;
            }
        }

        private static BuildProjectSetting CreateSettingForce()
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(BUILD_PROJECT_SETTING_PATH);
            if (!directoryInfo.Exists)
            {
                directoryInfo.Create();
            }

            BuildProjectSetting setting = CreateInstance<BuildProjectSetting>();
            AssetDatabase.CreateAsset(setting, BUILD_PROJECT_SETTING_FILE_PATH);

            return setting;
        }
        #endregion

        #region Find Build Project Setting

        private static BuildProjectSetting FindBuildProjectSetting()
        {
            BuildProjectSetting result = null;

            // Resources.FindObjectsOfTypeAll查找该类型的所有对象
            // 这个函数可以返回加载的Unity物体的任意类型，包含游戏对象、预制体、材质、网格、纹理等等
            BuildProjectSetting[] buildProjectSettings = Resources.FindObjectsOfTypeAll<BuildProjectSetting>();
            if (buildProjectSettings != null && buildProjectSettings.Length > 0)
            {
                result = buildProjectSettings[0];
            }

            if (!result)
            {
                string assetPath = Path.Combine(BUILD_PROJECT_SETTING_PATH, BUILD_PROJECT_SETTING_NAME + ".asset");
                result = AssetDatabase.LoadAssetAtPath<BuildProjectSetting>(assetPath);
            }

            return result;
        }

        #endregion
        #endregion
    }

    [System.Serializable]
    public class KeyValueItem
    {
        public string key;
        public string value;
    }
}
