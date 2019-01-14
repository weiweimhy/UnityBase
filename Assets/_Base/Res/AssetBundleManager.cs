using System.IO;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Text;

namespace BaseFramework
{
    public class AssetBundleManager : Singleton<AssetBundleManager>
    {
        private object obj = new object();

        private AssetBundleManifest assetBundleManifest;
        private Dictionary<string, AssetBundleUnit> caches;

        public override void OnSingletonInit()
        {
            base.OnSingletonInit();

            caches = new Dictionary<string, AssetBundleUnit>();

            AssetBundleUnit infoUnit = Load("StreamingAssets");
            assetBundleManifest = infoUnit.assetBundle.LoadAsset<AssetBundleManifest>("assetbundlemanifest");
            Release(infoUnit);
        }

        public override void OnSingletonDestroy()
        {
            base.OnSingletonDestroy();

            GameObject.Destroy(assetBundleManifest);
            assetBundleManifest = null;
            foreach (AssetBundleUnit unit in caches.Values)
            {
                Release(unit);
            }
            caches.Clear();
            caches = null;
        }

        public AssetBundleUnit Load(string name)
        {
            if (!caches.ContainsKey(name))
            {
                AssetBundleUnit assetBundleUnit = new AssetBundleUnit();
                assetBundleUnit.name = name;
                assetBundleUnit.assetBundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, name));
                assetBundleUnit.referenceCount++;
                if (assetBundleManifest != null)
                {
                    assetBundleUnit.dependencies = assetBundleManifest.GetAllDependencies(name);
                    assetBundleUnit.dependencies.ForEach((index, it) => {
                        Load(it);
                    });
                }
                caches.Add(name, assetBundleUnit);
            }

            return caches[name];
        }

        public void LoadAsync(string name, Action<AssetBundleUnit> loadAction)
        {
            List<string> assetNames = new List<string>();
            assetNames.Add(name);

            string[] dependencies = GetDependencies(name);
            if (dependencies != null)
            {
                dependencies.ForEach((indexer, it) => assetNames.Add(it));
            }

            IEnumerator<object>[] enumerators = new IEnumerator<object>[assetNames.Count];
            assetNames.ForEach((index, it) => enumerators[index] = Load(it, index == 0));

            TaskHelper.Create<CoroutineTask>()
                      .Delay(enumerators)
                      .Do(() => loadAction.InvokeGracefully(caches[name]))
                      .Execute();
        }

        private IEnumerator<object> Load(string assetBundleName, bool isRoot = false)
        {
            lock (obj)
            {
                if (!caches.ContainsKey(assetBundleName)
                    || caches[assetBundleName] == null)
                {
                    AssetBundleCreateRequest assetBundleCreateRequest = AssetBundle.LoadFromFileAsync(Path.Combine(Application.streamingAssetsPath, assetBundleName));

                    yield return assetBundleCreateRequest;

                    // wait one frame 
                    yield return null;

                    AssetBundleUnit unit = new AssetBundleUnit();
                    unit.referenceCount++;
                    unit.name = assetBundleName;
                    unit.assetBundle = assetBundleCreateRequest.assetBundle;
                    if (isRoot)
                    {
                        unit.dependencies = GetDependencies(assetBundleName);
                    }

                    caches.Add(assetBundleName, unit);
                }
                else
                {
                    caches[assetBundleName].referenceCount++;
                }

                yield return null;
            }
        }

        private AssetBundleUnit GetFormCache(string name)
        {
            if (caches.ContainsKey(name))
            {
                return caches[name];
            }
            return null;
        }

        private string[] GetDependencies(string assetBundleName)
        {
            if (assetBundleManifest != null)
            {
                return assetBundleManifest.GetAllDependencies(assetBundleName);
            }
            return null;
        }

        public void Release(string name,
                            bool unloadAllLoadedObjects = false)
        {
            if (caches.ContainsKey(name))
            {
                Release(caches[name]);
            }
        }

        public void Release(AssetBundleUnit bundleUnit,
                            bool unloadAllLoadedObjects = false)
        {
            if(bundleUnit == null)
            {
                Log.W(this, "Release asset bundle is null!");
                return;
            }

            string[] dependencies = bundleUnit.dependencies;
            dependencies.ForEach((index, it) => {
                if (caches.ContainsKey(it))
                {
                    Release(caches[it]);
                }
            });
            bundleUnit.referenceCount--;
            if (bundleUnit.referenceCount <= 0)
            {
                Unload(bundleUnit);
            }
        }

        private void Unload(AssetBundleUnit assetBundleUnit,
                           bool unloadAllLoadedObjects = false)
        {
            if (assetBundleUnit == null)
            {
                Log.W(this, "Unload asset bundle is null!");
                return;
            }

            if (caches.ContainsKey(assetBundleUnit.name))
            {
                caches.Remove(assetBundleUnit.name);
            }
            if (assetBundleUnit.assetBundle != null)
            {
                assetBundleUnit.assetBundle.Unload(unloadAllLoadedObjects);
            }
            assetBundleUnit.name = null;
            assetBundleUnit.assetBundle = null;
            assetBundleUnit.dependencies = null;
            assetBundleUnit.referenceCount = 0;
        }

        public void ExportCurrntMessage()
        {
            StringBuilder stringBuilder = new StringBuilder();
            caches.ForEach((key, value) => {
                if(value != null)
                {
                    stringBuilder.Append("AssetBundle path:" + key + ", referenceCount:" + value.referenceCount + "\n");
                }
                else
                {
                    Log.W(this, "ExportCurrntMessage {0} assetbundle is null", key);
                }
            });
            Log.I(this, stringBuilder.ToString());
        }
    }
}
