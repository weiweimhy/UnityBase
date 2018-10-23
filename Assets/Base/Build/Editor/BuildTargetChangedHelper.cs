using System;
using UnityEditor;
using UnityEditor.Build;

namespace Base.Editor.Build
{
    public class BuildTargetChangedHelper : IActiveBuildTargetChanged
    {
        public static Action<BuildTarget> changeCallback;

        public int callbackOrder { get { return 0; } }

        public void OnActiveBuildTargetChanged(BuildTarget previousTarget, BuildTarget newTarget)
        {
            if(changeCallback != null)
            {
                changeCallback(newTarget);
            }
        }

        public static void Switch2BuildTarget(BuildTarget buildTarget)
        {
            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Standalone, buildTarget);
        }
    }
}