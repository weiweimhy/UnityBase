using Base.LogUtil;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode.Custom;
using UnityEngine;

namespace Base.Editor.Build
{
    public class iOSBuildUtil
    {
        #region Build
        [MenuItem("Build/Export iOS XCode Project", false, 2)]
        public static void ExportXCodeProject()
        {
            BuildUtil.ExportTarget(BuildTarget.iOS);
        }
        #endregion

        #region Xcode project process
		private static readonly string unityProjectPath = Path.GetDirectoryName(Application.dataPath);

        private static readonly string xcodeEntitlementsFilePath = 
            Path.Combine(PBXProject.GetUnityTargetName(),"project.entitlements");

        private static string targetGuid = null;
        private static PBXProject pbxProject = null;
        private static string xcodeProjectExportPath = null;
        // eg: /Users/apple/exports/iOS/Unity-iPhone.xcodeproj/project.pbxproj
        private static string pbxProjectPath = null;

        [PostProcessBuild]
        public static void OnPostprocessBuild(BuildTarget buildTarget, string path)
        {
            if (buildTarget != BuildTarget.iOS)
            {
                return;
            }

            InitProjectInfo(path);

            // 设置自动签名
            SetCodeSign();
            // 开启系统Capabilities
            SetSystemCapabilities();
            // 添加Framework
            AddFramework();
            AddFrameworkSearchPath();
            // 添加tbd
            AddLibrary();
            AddLibrarySearchPath();
            // 
            AddHeaderSearchPath();
            // 修改属性
            FixProperty();
            // 修改plist.info
            FixPlist(path);
            // AddFile
            AddFile();

            File.WriteAllText(pbxProjectPath, pbxProject.WriteToString());
        }

        static void InitProjectInfo(string path)
        {
            // eg: /Users/apple/exports/iOS
            xcodeProjectExportPath = path;

            // eg: /Users/apple/exports/iOS/Unity-iPhone.xcodeproj/project.pbxproj
            pbxProjectPath = PBXProject.GetPBXProjectPath(path);

            pbxProject = new PBXProject();
            // proj.ReadFromString(File.ReadAllText(projPath));
            pbxProject.ReadFromFile(pbxProjectPath);

            // PBXProject.GetUnityTargetName() = "Unity-iphone"
            targetGuid = pbxProject.TargetGuidByName(PBXProject.GetUnityTargetName());
        }

        static void SetCodeSign()
        {
            pbxProject.SetTargetAttributes("DevelopmentTeam", BuildProjectSetting.instance.signTeamID);
            pbxProject.SetTargetAttributes("ProvisioningStyle", "Automatic");
        }

        static void AddFramework()
        {
            List<string> frameworks = BuildProjectSetting.instance.frameworks;
            if (frameworks != null)
            {
                for(int i = 0;i < frameworks.Count; ++i)
                {
                    pbxProject.AddFrameworkToProject(targetGuid, frameworks[i], false);
                }
            }

        }

        static void AddFrameworkSearchPath() {
            // pbxProject.AddBuildProperty(targetGuid, "FRAMEWORK_SEARCH_PATHS", "xxxx");
        }

        static void AddLibrary()
        {
            List<string> libraries = BuildProjectSetting.instance.libraries;
            if(libraries != null)
            {
                for(int i = 0;i < libraries.Count; ++i)
                {
                    // 参数1 path:相对PBXSourceTree.Sdk的路径
                    // 参数2 projectPath:相对 Xcode导出工程的路径
                    // PBXSourceTree.Sdk eg：
                    //                   /Applications/Xcode.app/Contents/Developer/Platforms/iPhoneOS.platform/Developer/SDKs/
                    string fileLibrary = pbxProject.AddFile("/usr/lib/" + libraries[i],
                                                            "Frameworks/" + libraries[i], PBXSourceTree.Sdk);
                    pbxProject.AddFileToBuild(targetGuid, fileLibrary);
                }
            }
        }

        static void AddLibrarySearchPath()
        {
            // pbxProject.AddBuildProperty(targetGuid, "LIBRARY_SEARCH_PATHS", "xxxx");
        }

        static void AddHeaderSearchPath()
        {
            // pbxProject.AddBuildProperty(targetGuid, "HEADER_SEARCH_PATHS", "xxxx");
        }

        static void FixProperty()
        {
            List<KeyValueItem> properties = BuildProjectSetting.instance.properties;
            if(properties != null)
            {
                for(int i = 0;i < properties.Count; ++i)
                {
                    pbxProject.AddBuildProperty(targetGuid, properties[i].key, properties[i].value);
                }
            }
        }

        static void FixPlist(string path)
        {
            List<KeyValueItem> plists = BuildProjectSetting.instance.plists;
            if(plists != null && plists.Count > 0)
            {
                var plistPath = Path.Combine(path, "Info.plist");
                PlistDocument plist = new PlistDocument();
                plist.ReadFromString(File.ReadAllText(plistPath));

                PlistElementDict rootDic = plist.root;

                for(int i = 0;i< plists.Count; ++i)
                {
                    rootDic.SetString(plists[i].key, plists[i].value);
                }

                File.WriteAllText(plistPath, plist.WriteToString());
            }
        }

        static void AddFile()
        {
			List<Object> files = BuildProjectSetting.instance.files;
            if(files != null)
            {
                for(int i = 0;i < files.Count; ++i)
                {
					// eg:Assets/xxx/xxxx.xxx
					string fileRelativePath = AssetDatabase.GetAssetPath(files[i]);
					string fileName = Path.GetFileName (fileRelativePath);
					string sourceFilePath = Path.Combine(unityProjectPath,fileRelativePath);
					string destFilePath = Path.Combine(xcodeProjectExportPath, fileName);
					File.Copy(sourceFilePath, destFilePath);
					// 不调用AddFileToBuild IPA运行时会Crash
					pbxProject.AddFileToBuild(targetGuid, pbxProject.AddFile(destFilePath, fileName));
                }
            }
        }

        static void SetSystemCapabilities()
        {
            List<iOSCapabilityType> capabilitys = BuildProjectSetting.instance.capabilitys;
            if (capabilitys != null)
            {
                for(int i = 0; i< capabilitys.Count; ++i)
                {
                    PBXCapabilityType type = iOSCapabilityTypeHelper.GetPBXCapabilityType(capabilitys[i]);
                    if (type != null)
                    {
                        pbxProject.AddCapability(targetGuid,
                            iOSCapabilityTypeHelper.GetPBXCapabilityType(capabilitys[i]), xcodeEntitlementsFilePath);
                    }
                }
            }
        }

        #endregion
    }
}
