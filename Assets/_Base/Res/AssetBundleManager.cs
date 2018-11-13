using System.IO;
using UnityEngine;
using System;
using System.Collections.Generic;

namespace BaseFramework
{
    public class AssetBundleManager : Singleton<AssetBundleManager>
    {
        private AssetBundleManifest assetBundleManifest;
        private Dictionary<string, AssetBundleUnit> caches;

        public override void OnSingletonInit()
        {
            base.OnSingletonInit();

            caches = new Dictionary<string, AssetBundleUnit>();

            AssetBundleUnit infoUnit = Load("StreamingAssets");
            assetBundleManifest = infoUnit.assetBundle.LoadAsset<AssetBundleManifest>("assetbundlemanifest");
            Unload(infoUnit, false, false);
        }

        public override void OnSingletonDestroy()
        {
            base.OnSingletonDestroy();

            GameObject.Destroy(assetBundleManifest);
            assetBundleManifest = null;
            foreach(AssetBundleUnit unit in caches.Values)
            {
                Unload(unit,true,true);
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
                if (assetBundleManifest != null)
                {
                    assetBundleUnit.dependencies = assetBundleManifest.GetAllDependencies(name);
                    foreach (string dependency in assetBundleUnit.dependencies)
                    {
                        Load(dependency);
                    }
                }
                caches.Add(name, assetBundleUnit);
            }

            return caches[name];
        }

        public AsyncOperation LoadAsync(string name, Action<AssetBundleUnit> finishAction)
        {
            if (!caches.ContainsKey(name))
            {
                AssetBundleCreateRequest assetBundleCreateRequest = AssetBundle.LoadFromFileAsync(Path.Combine(Application.streamingAssetsPath, name));
                
                TaskHelper.Create<CoroutineTask>()
                    .Delay(assetBundleCreateRequest)
                    .Do(() => {
                        AssetBundleUnit assetBundleUnit = new AssetBundleUnit();
                        assetBundleUnit.name = name;
                        assetBundleUnit.assetBundle = assetBundleCreateRequest.assetBundle;
                        if (assetBundleManifest != null)
                        {
                            assetBundleUnit.dependencies = assetBundleManifest.GetAllDependencies(name);
                            if (assetBundleUnit.dependencies.Length > 0)
                            {
                                foreach (string dependency in assetBundleUnit.dependencies)
                                {
                                    Load(dependency);
                                }
                            }
                            else
                            {
                                finishAction.InvokeGracefully(assetBundleUnit);
                            }
                        }
                        else
                        {
                            finishAction.InvokeGracefully(assetBundleUnit);
                        }

                        assetBundleCreateRequest = null;
                    });

                return assetBundleCreateRequest;
            }

            finishAction.InvokeGracefully(caches[name]);

            return null;
        } 

        public void Unload(AssetBundleUnit assetBundleUnit,
                                      bool unloadDependencies = false,
                                      bool unloadAllLoadedObjects = true)
        {
            if (unloadDependencies)
            {
                string[] dependencies = assetBundleUnit.dependencies;
                if (dependencies != null)
                {
                    foreach (string dependency in dependencies)
                    {
                        if(caches.ContainsKey(dependency))
                        {
                            Unload(caches[dependency], false, unloadAllLoadedObjects);
                        }
                    }
                }
            }

            if(caches.ContainsValue(assetBundleUnit))
                caches.Remove(assetBundleUnit.name);
            if(assetBundleUnit.assetBundle != null)
            {
                assetBundleUnit.assetBundle.Unload(unloadAllLoadedObjects);
            }
            assetBundleUnit.name = null;
            assetBundleUnit.assetBundle = null;
            assetBundleUnit.dependencies = null;
        }
    }
}
