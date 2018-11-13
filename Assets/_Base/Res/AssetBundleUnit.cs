using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BaseFramework
{
    public struct AssetBundleUnit
    {
        public string name;
        public AssetBundle assetBundle;
        public string[] dependencies;
    }
}
