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
            RecycleTestItem item = PoolHelper.Create(prefab, MonoPoolType.AutoDispose);
            item.transform
                .Parent(itemParent, false)
                .LastSibling()
                .Active();
            item.index = items.Count;
            items.Add(item);
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
    }
}
