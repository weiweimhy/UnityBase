using System.Collections;
using UnityEngine;

namespace BaseFramework
{
    public class ResHelper
    {
        private static string TAG = "ResHelper";

        public static AssetBundleUnit LoadAssetBundleUnit(string path)
        {
            AssetBundleUnit assetBundleUnit = AssetBundleManager.instance.Load(path);

            return assetBundleUnit;
        }

        public static void LoadAssetBundleUnitAsyc(string path, System.Action<AssetBundleUnit> finish)
        {
            AssetBundleManager.instance.LoadAsync(path, finish);
        }

        public static T LoadAsset<T>(string path,
                                     string name) where T : Object
        {
            if (IsResources(path))
            {
                T asset = Resources.Load<T>(GetResourcesName(path, name));
                return asset;
            }

            AssetBundleUnit assetBundleUnit = LoadAssetBundleUnit(path);
            T ret = LoadAssetFromAssetBundleUnit<T>(assetBundleUnit, name);

            return ret;
        }

        public static void LoadAssetAsyc<T>(string path,
                                            string name,
                                            System.Action<T> action,
                                            bool release = false) where T : Object
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
                LoadAssetBundleUnitAsyc(path, (assetBundleUnit) => {
                    LoadAssetFromAssetBundleUnitAsyc<T>(assetBundleUnit, name, (asset) => {
                        action.InvokeGracefully(asset);
                    }, release);
                });
            }
        }

        public static T LoadAssetFromAssetBundleUnit<T>(AssetBundleUnit assetBundleUnit,
                                                        string name,
                                                        bool release = false) where T : Object
        {
            if (assetBundleUnit != null && assetBundleUnit.assetBundle != null)
            {
                T asset = assetBundleUnit.assetBundle.LoadAsset<T>(name);
                if (release)
                {
                    // 立即卸载AssetBundle会报错
                    // error message:
                    // Cancelling DisplayDialog because it was run from a thread that is not the main thread: Opening file failed Opening file
                    TaskHelper.Create<CoroutineTask>()
                              .Delay(1)
                              .Do(() => {
                                  AssetBundleManager.instance.Release(assetBundleUnit);
                                  Log.I(TAG, "Release assetbundleunit by time delay");
                              });
                    }
                return asset;
            }

            Log.W(TAG, "Load Asset {0} from {1} is null", name, assetBundleUnit.name);

            return null;
        }

        public static void LoadAssetFromAssetBundleUnitAsyc<T>(AssetBundleUnit assetBundleUnit,
                                                               string name,
                                                               System.Action<T> finish,
                                                               bool release = false) where T : Object
        {
            if (assetBundleUnit != null && assetBundleUnit.assetBundle != null)
            {
                TaskHelper.Create<CoroutineTask>()
                          .Delay(Load(assetBundleUnit, name, finish))
                          .Do(() => {
                              if(release)
                              {
                                  AssetBundleManager.instance.Release(assetBundleUnit);
                              }
                          })
                          .Execute();
            }
            else
            {
                Log.W(TAG, "Load Asset {0} from {1} is null", name, assetBundleUnit.name);
                finish.InvokeGracefully(null);
            }
        }

        private static IEnumerator Load<T>(AssetBundleUnit assetBundleUnit,
                                           string name,
                                           System.Action<T> action) where T : Object
        {
            if (assetBundleUnit != null && assetBundleUnit.assetBundle != null)
            {
                AssetBundleRequest request = assetBundleUnit.assetBundle.LoadAssetAsync<T>(name);
                yield return request;
                action.InvokeGracefully(request.asset);
            }
            else
            {
                action.InvokeGracefully(null);
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
