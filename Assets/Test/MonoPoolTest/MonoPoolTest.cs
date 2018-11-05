using UnityEngine;
using UnityEngine.UI;
using BaseFramework;
using System.Collections.Generic;

namespace BaseFramework.Test
{

    public class MonoPoolTest : MonoBehaviour
    {
        public RecycleTestItem prefab;
        public Transform itemParent;
        private List<RecycleTestItem> items = new List<RecycleTestItem>(); 

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Create()
        {
            RecycleTestItem item = PoolHelper.Create(prefab);
            item.transform
                .Parent(itemParent, false)
                .SetAsLastSibling();
            items.Add(item);
            item.index = items.Count;
        }

        public void RecycleLast()
        {
            var item = items.RemoveLast();
            if(item.IsNotNull())
            {
                item.Dispose();
            }
        }
        
        public void RecycleAll()
        {
            items.ForEach(it => it.Dispose());
            items.Clear();
        }

        private void OnDestroy()
        {
            PoolHelper.Dispose<RecycleTestItem>();
        }
    }
}
