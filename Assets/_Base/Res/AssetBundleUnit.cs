using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BaseFramework
{
    public class AssetBundleUnit
    {
        public string name { get; set; }
        public AssetBundle assetBundle { get; set; }
        public string[] dependencies;
        public int referenceCount { get; set; }
    }
}
