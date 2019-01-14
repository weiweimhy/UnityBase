using System.IO;
using UnityEditor;
using UnityEngine;

namespace BaseFramework.Build
{
    [InitializeOnLoad]
    public static class iOSPodUtil
    {
        private static string TAG = "iOSPodUtil";

        private static string[] POD_SEARCH_PATHS = new string[]
        {
            "/usr/local/bin",
            "/usr/bin"
        };
        private static string POD_EXECUTABLE = "pod";

        private static string sourcePath = "/_Base/Build/iOS/Res/Podfile";

        static iOSPodUtil()
        {
            BuildUtil.onPostprocessBuild += ExecutePod;
        }

        private static string FindPodTool()
        {
            foreach (string path in POD_SEARCH_PATHS)
            {
                string text = Path.Combine(path, POD_EXECUTABLE);
                Log.I("IOSResolver", "Searching for CocoaPods tool in " + text);
                if (File.Exists(text))
                {
                    Log.I("IOSResolver", "Found CocoaPods tool in " + text);
                    return text;
                }
            }

            return null;
        }

        private static void ExecutePod(BuildTarget buildTarget, string path)
        {
            if (CopyPodfile(path))
            {
                string podPath = FindPodTool();
                if (!podPath.IsEmptyOrNull())
                {
                    ExecutedPod(podPath, path);
                }
                else
                {
                    Log.E(TAG, "pod not install!");
                }
            }
        }

        private static bool CopyPodfile(string path)
        {
            // check podfile
            FileInfo fileInfo = new FileInfo(sourcePath);
            if (!fileInfo.Exists)
            {
                Log.E(TAG, "{0} not exists!", sourcePath);
                return false;
            }

            File.Copy(Application.dataPath + sourcePath, Path.Combine(path, "Podfile"));

            return true;
        }

        private static bool ExecutedPod(string podPath, string projectPath)
        {
            string commandLine = string.Format("cd {0} && {1} update && {2} install", projectPath, podPath, podPath);
            Log.I(TAG, "start executed command\n{0}", commandLine);
            CommandUtil.CommandResult result = CommandUtil.ExecuteCommand(commandLine);

            if (result.resultCode == 0)
            {
                return true;
            }

            Log.E(TAG, "executed command error!\n{0}\n{1}", result.output, result.error);

            return false;
        }
    }
}
