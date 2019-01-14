using System.IO;
using UnityEditor;
using UnityEngine;

namespace BaseFramework.Lunch
{
    public class LaunchSetting : ScriptableObject
    {
        [Header("Lunch Scene Name")]
        public SceneAsset lunchScene;

        #region BuildProjectSetting
        private static readonly string LUNCH_SETTING_PATH = "Assets/_Base/Lunch/LunchSetting";
        private static readonly string LUNCH_SETTING_NAME = "LunchSetting";

        private static readonly string LUNCH_SETTING_FILE_PATH = Path.Combine(LUNCH_SETTING_PATH, LUNCH_SETTING_NAME + ".asset");

        #region singleton

        private static LaunchSetting _instance;

        public static LaunchSetting instance
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

        [MenuItem("Base/Lunch/Edit Lunch Setting", false, 0)]
        public static void CreateSetting()
        {
            if (instance != null)
            {
                Selection.activeObject = instance;
            }
        }

        private static LaunchSetting CreateSettingForce()
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(LUNCH_SETTING_PATH);
            if (!directoryInfo.Exists)
            {
                directoryInfo.Create();
            }

            LaunchSetting setting = CreateInstance<LaunchSetting>();
            AssetDatabase.CreateAsset(setting, LUNCH_SETTING_FILE_PATH);

            return setting;
        }
        #endregion

        #region Find Build Project Setting

        private static LaunchSetting FindBuildProjectSetting()
        {
            LaunchSetting result = null;

            // Resources.FindObjectsOfTypeAll查找该类型的所有对象
            // 这个函数可以返回加载的Unity物体的任意类型，包含游戏对象、预制体、材质、网格、纹理等等
            LaunchSetting[] buildProjectSettings = Resources.FindObjectsOfTypeAll<LaunchSetting>();
            if (buildProjectSettings != null && buildProjectSettings.Length > 0)
            {
                result = buildProjectSettings[0];
            }

            if (!result)
            {
                result = AssetDatabase.LoadAssetAtPath<LaunchSetting>(LUNCH_SETTING_FILE_PATH);
            }

            return result;
        }

        #endregion
        #endregion

        #region Clear Lunch Setting
        [MenuItem("Base/Lunch/Clear Lunch Setting", false, 0)]
        private static void ClearSetting()
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(LUNCH_SETTING_PATH);
            if (!directoryInfo.Exists)
            {
                directoryInfo.Create();
            }
            AssetDatabase.DeleteAsset(LUNCH_SETTING_FILE_PATH);
            _instance = null;
        }
        #endregion

        #region Reset Lunch Setting
        [MenuItem("Base/Lunch/Reset Lunch Setting", false, 0)]
        private static void ResetSetting()
        {
            ClearSetting();
            CreateSetting();
        }
        #endregion
    }

}
