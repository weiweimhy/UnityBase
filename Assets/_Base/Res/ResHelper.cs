using UnityEngine;

namespace BaseFramework
{
    public class ResHelper
    {
        public static T LoadAsset<T>(string path, string name) where T : Object
        {
            if (path.StartsWith("Resources"))
            {
                name = name.AddPrefix(path.Replace("Resources/", ""));
                T asset = Resources.Load<T>(name);
                return asset;
            }

            AssetBundleUnit assetBundleUnit = AssetBundleManager.instance.Load(path);
            if (assetBundleUnit.assetBundle != null)
            {
                T asset = assetBundleUnit.assetBundle.LoadAsset<T>(name);
                AssetBundleManager.instance.Unload(assetBundleUnit, true, false);
                return asset;
            }

            return null;
        }

        public static void LoadAssetAsyc<T>(string path, string name, System.Action<T> action) where T : Object
        {
            if (path.StartsWith("Resources"))
            {
                name = name.AddPrefix(path.Replace("Resources/", ""));
                ResourceRequest request = Resources.LoadAsync<T>(name);
                TaskHelper.Create<CoroutineTask>()
                    .Delay(request)
                    .Do(() => {
                        action.InvokeGracefully(request.asset.As<T>());
                    });
            }
            else
            {
                AssetBundleManager.instance.LoadAsync(path, (assetBundleUnit) => {
                    if (assetBundleUnit.assetBundle != null)
                    {
                        T asset = assetBundleUnit.assetBundle.LoadAsset<T>(name);
                        AssetBundleManager.instance.Unload(assetBundleUnit, true, false);
                        action.InvokeGracefully(asset);
                    }
                });
            }
        }
    }
}
