using UnityEngine;
using UnityEditor;
using BaseFramework.Build;
using System.IO;

#if UNITY_ANDROID && UNITY_EDITOR
namespace BaseFramework.ThirdPlugin.Firebase
{
    [InitializeOnLoad]
    public static class CopyGoogleServices
    {
        private static string googleServecesFileName = "google-services.json";
        private static readonly string googleServecesFilePath =
            Application.dataPath + "/_ThirdPlugins/Firebase/Analytics/Res/" + googleServecesFileName;

        static CopyGoogleServices()
        {
            BuildUtil.onPostprocessBuild += CopyGoogleServecesFile;
        }

        private static void CopyGoogleServecesFile(BuildTarget buildTarget, string path)
        {
            if(buildTarget != BuildTarget.Android)
            {
                return;
            }

            FileInfo fileInfo = new FileInfo(googleServecesFilePath);
            if (!fileInfo.Exists)
            {
                Log.E("CopyGoogleServices", "google-services.json not exists!");
                return;
            }

            File.Copy(googleServecesFilePath, Path.Combine(path, Application.productName + "/" + googleServecesFileName));
            Log.I("CopyGoogleServices", "copy google-services.json success!");

            
        }
    }
}
#endif
