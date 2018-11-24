using UnityEngine;

namespace BaseFramework
{
    public class ResHelper
    {
        public static T LoadAsset<T>(string path, string name) where T : Object
        {
            if (IsResources(path))
            {
                T asset = Resources.Load<T>(GetResourcesName(path, name));
                return asset;
            }

            AssetBundleUnit assetBundleUnit = AssetBundleManager.instance.Load(path);
            if (assetBundleUnit.assetBundle != null)
            {
                T asset = assetBundleUnit.assetBundle.LoadAsset<T>(name);
                AssetBundleManager.instance.Release(assetBundleUnit);
                return asset;
            }

            return null;
        }

        public static void LoadAssetAsyc<T>(string path, string name, System.Action<T> action) where T : Object
        {
            if (IsResources(path))
            {
                ResourceRequest request = Resources.LoadAsync<T>(GetResourcesName(path, name));
                TaskHelper.Create<CoroutineTask>()
                    .Delay(request)
                    .Do(() => {
                        action.InvokeGracefully(request.asset.As<T>());
                    })
                    .Execute();
            }
            else
            {
                AssetBundleManager.instance.LoadAsync(path, (assetBundleUnit) => {
                    if (assetBundleUnit.assetBundle != null)
                    {
                        T asset = assetBundleUnit.assetBundle.LoadAsset<T>(name);
                        AssetBundleManager.instance.Release(assetBundleUnit);
                        action.InvokeGracefully(asset);
                    }
                });
            }
        }

        private static bool IsResources(string name)
        {
            return name.StartsWith("Resources");
        }

        private static string GetResourcesName(string path, string name)
        {
            if (path.StartsWith("Resources/"))
            {
                name = name.AddPrefix(path.Replace("Resources/", ""));
            }
            return name;
        }
    }
}
