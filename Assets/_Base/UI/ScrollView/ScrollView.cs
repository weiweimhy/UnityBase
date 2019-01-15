using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BaseFramework.UI
{
    public class ScrollView : MonoBehaviour, IDragHandler, IEndDragHandler
    {
        public enum ScrollDirection
        {
            TopToBottom,
            BottomToTop,
            LeftToRight,
            RightToLeft,
        }

        public ScrollDirection scrollDirection = ScrollDirection.TopToBottom;
        public ScrollViewPrefabData[] prefabDatas;
        [Tooltip("margin[0].x is left, margin[1].y is bottom\nmargin[1].x is right, margin[0].y is top")]
        public Vector2[] margin = new Vector2[] { Vector2.zero, Vector2.zero };
        public Vector2[] padding = new Vector2[] { Vector2.zero, Vector2.zero };
        // 预留空间，用来提前创建Item，延迟销毁Item
        public float reservedSize = 100;
        // 检测滑动停止的距离
        public float minRollDistance = 0.1f;

        private ScrollViewAdapter adapter;

        private ScrollRect scrollRect;
        private RectTransform contentTransform;
        private RectTransform viewPortTransform;

        private float viewPortSize;

        private List<ScrollViewItem> items;

        private bool isInitFinish = false;

        private bool isDrag = false;
        private bool isRolling = false;
        private float lastPosition;

        public Vector2 viewPortSzie
        {
            get
            {
                return viewPortTransform.rect.size;
            }
        }

        public void Init(ScrollViewAdapter adapter)
        {
            if (isInitFinish)
            {
                return;
            }

            this.adapter = adapter;
            adapter.Init(this, prefabDatas);

            scrollRect = GetComponent<ScrollRect>();
            contentTransform = scrollRect.content;
            viewPortTransform = scrollRect.viewport;

            InitScrollRect();
            InitViewPort();
            AdjustContent();

            UpdateContentSize();
            ScrollTo(0);
            contentTransform.anchoredPosition = Vector2.zero;

            isInitFinish = true;
        }

        private void InitScrollRect()
        {
            scrollRect.vertical = IsVertical();
            scrollRect.horizontal = !scrollRect.vertical;
        }

        private void InitViewPort()
        {
            viewPortTransform.offsetMin = new Vector2(margin[0].x, margin[1].y);
            viewPortTransform.offsetMax = new Vector2(-margin[1].x, -margin[0].y);

            if (IsVertical())
            {
                viewPortSize = viewPortTransform.rect.height;
            }
            else
            {
                viewPortSize = viewPortTransform.rect.width;
            }
        }

        public void ScrollTo(int index, float offset = 0)
        {
            if (index < 0 || index >= adapter.GetCount())
            {
                Debug.LogWarning("[ScrollView] scroll to {0}, index is invalid!");
                index = 0;
            }

            UpdateView(index, offset);
        }

        public bool IsVertical()
        {
            return scrollDirection == ScrollDirection.TopToBottom || scrollDirection == ScrollDirection.BottomToTop;
        }

        private void AdjustContent()
        {
            switch (scrollDirection)
            {
                case ScrollDirection.TopToBottom:
                    contentTransform.ChangeAnchorAndPivot(LayoutUtil.LayoutAnchorType.TopStretch);
                    break;
                case ScrollDirection.BottomToTop:
                    contentTransform.ChangeAnchorAndPivot(LayoutUtil.LayoutAnchorType.BottomStretch);
                    break;
                case ScrollDirection.LeftToRight:
                    contentTransform.ChangeAnchorAndPivot(LayoutUtil.LayoutAnchorType.StretchLeft);
                    break;
                case ScrollDirection.RightToLeft:
                    contentTransform.ChangeAnchorAndPivot(LayoutUtil.LayoutAnchorType.StretchRight);
                    break;
            }
            contentTransform.offsetMin = Vector2.zero;
            contentTransform.offsetMax = Vector2.zero;
        }

        internal void UpdateContentSize()
        {
            float contentTotalSize = adapter.GetTotalSize();
            // padding
            contentTotalSize += padding[0].y;
            contentTotalSize += padding[1].y;

            if (IsVertical())
            {
                if (contentTransform.rect.height != contentTotalSize)
                {
                    contentTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, contentTotalSize);
                }
            }
            else
            {
                if (contentTransform.rect.width != contentTotalSize)
                {
                    contentTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, contentTotalSize);
                }
            }
        }

        private void UpdateView(int index, float offset = 0)
        {
            if (adapter == null)
            {
                throw new NullReferenceException("[ScrollView] UpdateView adapter is null!");
            }

            if (items == null)
            {
                items = new List<ScrollViewItem>();
            }
            else
            {
                foreach (ScrollViewItem item in items)
                {
                    adapter.RecycleInstance(item);
                }
                items.Clear();
            }

            switch (scrollDirection)
            {
                case ScrollDirection.TopToBottom:
                    UpdateViewTopToBottom(index, offset);
                    break;
                case ScrollDirection.BottomToTop:
                    UpdateViewBottomToTop(index, offset);
                    break;
                case ScrollDirection.LeftToRight:
                    UpdateViewLeftToRight(index, offset);
                    break;
                case ScrollDirection.RightToLeft:
                    UpdateViewRightToLeft(index, offset);
                    break;
            }
        }

        private void UpdateViewTopToBottom(int index, float offset)
        {
            float beforeSize = adapter.GetSizeOffset(index) + padding[0].y;

            // init behind [include self]
            float totalSize = viewPortTransform.rect.height;
            float itemsSize = 0;
            float anchorOffsetTemp = -offset - beforeSize;
            int indexTemp = index;
            while (itemsSize < totalSize + reservedSize - offset && indexTemp < adapter.GetCount())
            {
                ScrollViewItem item = adapter.CreateInstance(indexTemp++);
                float size = AdjustItemViewTopToBottom(item, anchorOffsetTemp);

                anchorOffsetTemp -= size;
                itemsSize += size;
                items.Add(item);
            }

            // init before
            anchorOffsetTemp = -offset - beforeSize - padding[0].y;
            itemsSize = 0;
            indexTemp = index;
            while (itemsSize < reservedSize + offset && indexTemp > 0)
            {
                ScrollViewItem item = adapter.CreateInstance(--indexTemp);
                float size = AdjustItemViewTopToBottom(item, anchorOffsetTemp, true);

                anchorOffsetTemp += size;
                itemsSize += size;
                items.Insert(0, item);
            }

            contentTransform.anchoredPosition = new Vector2(contentTransform.anchoredPosition.x, beforeSize);
        }

        private float AdjustItemViewTopToBottom(ScrollViewItem item, float anchoredPosition, bool useSelfSize = false)
        {
            float size = item.rectTransform.rect.height;

            item.rectTransform.SetParent(contentTransform, false);
            item.rectTransform.ChangeAnchors(LayoutUtil.LayoutAnchorType.TopCenter);
            item.rectTransform.ChangePivot(LayoutUtil.LayoutPivotType.CenterTop);
            item.rectTransform.anchoredPosition = new Vector2(0, anchoredPosition + (useSelfSize ? size : 0));

            return size;
        }

        private void UpdateViewBottomToTop(int index, float offset)
        {
            float beforeSize = adapter.GetSizeOffset(index) + padding[1].y;

            // init behind [include self]
            float totalSize = viewPortTransform.rect.height;
            float itemsSize = 0;
            float anchorOffsetTemp = offset + beforeSize;
            int indexTemp = index;
            while (itemsSize < totalSize + reservedSize - offset && indexTemp < adapter.GetCount())
            {
                ScrollViewItem item = adapter.CreateInstance(indexTemp++);
                float size = AdjustItemViewBottomToTop(item, anchorOffsetTemp);

                anchorOffsetTemp += size;
                itemsSize += size;
                items.Add(item);
            }

            // init before
            anchorOffsetTemp = offset + beforeSize;
            itemsSize = 0;
            indexTemp = index;
            while (itemsSize < reservedSize + offset && indexTemp > 0)
            {
                ScrollViewItem item = adapter.CreateInstance(--indexTemp);
                float size = AdjustItemViewBottomToTop(item, anchorOffsetTemp, true);

                anchorOffsetTemp -= size;
                itemsSize += size;
                items.Insert(0, item);
            }

            contentTransform.anchoredPosition = new Vector2(contentTransform.anchoredPosition.x, -beforeSize);
        }

        private float AdjustItemViewBottomToTop(ScrollViewItem item, float anchoredPosition, bool useSelfSize = false)
        {
            float size = item.rectTransform.rect.height;

            item.rectTransform.SetParent(contentTransform, false);
            item.rectTransform.ChangeAnchors(LayoutUtil.LayoutAnchorType.BottomCenter);
            item.rectTransform.ChangePivot(LayoutUtil.LayoutPivotType.CenterBottom);
            item.rectTransform.anchoredPosition = new Vector2(0, anchoredPosition - (useSelfSize ? size : 0));

            return size;
        }

        private void UpdateViewLeftToRight(int index, float offset)
        {

        }

        private void UpdateViewRightToLeft(int index, float offset)
        {

        }

        private float AdjustItemView(ScrollViewItem item, float anchorOffset)
        {
            // set parent
            item.rectTransform.SetParent(contentTransform, false);

            if (scrollDirection == ScrollDirection.TopToBottom)
            {
                item.rectTransform.ChangeAnchors(new Vector2(0.5f, 1), new Vector2(0.5f, 1));
                item.rectTransform.ChangePivot(new Vector2(0.5f, 1));
                item.rectTransform.anchoredPosition = new Vector2(0, anchorOffset);
                return item.rectTransform.rect.height;
            }
            if (scrollDirection == ScrollDirection.BottomToTop)
            {
                item.rectTransform.ChangeAnchors(new Vector2(0.5f, 0), new Vector2(0.5f, 0));
                item.rectTransform.ChangePivot(new Vector2(0.5f, 0));
                item.rectTransform.anchoredPosition = new Vector2(0, -anchorOffset);
                return item.rectTransform.rect.height;
            }
            if (scrollDirection == ScrollDirection.LeftToRight)
            {
                item.rectTransform.ChangeAnchors(new Vector2(0, 0.5f), new Vector2(0, 0.5f));
                item.rectTransform.ChangePivot(new Vector2(0, 0.5f));
                item.rectTransform.anchoredPosition = new Vector2(-anchorOffset, 0);
                return item.rectTransform.rect.width;
            }
            // scrollDirection == ScrollDirection.RightToLeft
            item.rectTransform.ChangeAnchors(new Vector2(1, 0.5f), new Vector2(1, 0.5f));
            item.rectTransform.ChangePivot(new Vector2(1, 0.5f));
            item.rectTransform.anchoredPosition = new Vector2(anchorOffset, 0);
            return item.rectTransform.rect.width;
        }

        private void Update()
        {
            if (!isInitFinish)
            {
                return;
            }

            if (!isRolling)
            {
                lastPosition = IsVertical() ? contentTransform.anchoredPosition.y : contentTransform.anchoredPosition.x;
                return;
            }

            switch (scrollDirection)
            {
                case ScrollDirection.TopToBottom:
                    UpdateTopToBottom();
                    break;
                case ScrollDirection.BottomToTop:
                    UpdateBottomToTop();
                    break;
                case ScrollDirection.LeftToRight:
                    UpdateLeftToRight();
                    break;
                case ScrollDirection.RightToLeft:
                    UpdateRightToLeft();
                    break;
            }

            if (isRolling && !isDrag)
            {
                float currentPosition = IsVertical() ? contentTransform.anchoredPosition.y : contentTransform.anchoredPosition.x;
                if (Mathf.Abs(lastPosition - currentPosition) < minRollDistance)
                {
                    Debug.Log("[ScrollView] Stop roll");
                    scrollRect.StopMovement();
                    isRolling = false;
                }
                else
                {
                    lastPosition = currentPosition;
                }
            }
        }

        private void UpdateTopToBottom()
        {
            // top
            int firstShowItemIndex = -1;
            for (int i = 0; i < items.Count; ++i)
            {
                if (items[i].rectTransform.localPosition.y + contentTransform.anchoredPosition.y - items[i].rectTransform.rect.height > reservedSize)
                {
                    firstShowItemIndex = i;
                    break;
                }
            }
            for (int i = 0; i <= firstShowItemIndex; ++i)
            {
                ScrollViewItem recycleItem = items[0];
                adapter.RecycleInstance(recycleItem);
                items.RemoveAt(0);
            }
            if (items[0].rectTransform.localPosition.y + contentTransform.anchoredPosition.y < reservedSize)
            {
                int index = items[0].index;
                float emptySize = -(items[0].rectTransform.localPosition.y + contentTransform.anchoredPosition.y) + reservedSize;
                ScrollViewItem preItem = items[0];
                while (index > 0 && emptySize > 0)
                {
                    ScrollViewItem item = adapter.CreateInstance(--index);
                    AdjustItemView(item, preItem.rectTransform.anchoredPosition.y + item.rectTransform.rect.height);
                    emptySize -= item.rectTransform.rect.height;
                    preItem = item;
                    items.Insert(0, item);
                }
            }

            // bottom
            int lastShowItemIndex = items.Count;
            for (int i = items.Count - 1; i >= 0; --i)
            {
                if (items[i].rectTransform.localPosition.y + contentTransform.anchoredPosition.y < -viewPortSize - reservedSize)
                {
                    lastShowItemIndex = i;
                    break;
                }
            }
            for (int i = 0; i < items.Count - lastShowItemIndex; ++i)
            {
                ScrollViewItem recycleItem = items[items.Count - 1];
                adapter.RecycleInstance(recycleItem);
                items.RemoveAt(items.Count - 1);
            }
            if (items[items.Count - 1].rectTransform.localPosition.y + contentTransform.anchoredPosition.y - items[items.Count - 1].rectTransform.rect.height > -viewPortSize - reservedSize)
            {
                int index = items[items.Count - 1].index;
                float emptySize = viewPortSize + reservedSize + (items[items.Count - 1].rectTransform.localPosition.y + contentTransform.anchoredPosition.y - items[items.Count - 1].rectTransform.rect.height);
                ScrollViewItem preItem = items[items.Count - 1];
                while (emptySize > 0 && index < adapter.GetCount())
                {
                    ScrollViewItem item = adapter.CreateInstance(++index);
                    AdjustItemView(item, preItem.rectTransform.anchoredPosition.y - preItem.rectTransform.rect.height);
                    emptySize -= preItem.rectTransform.rect.height;
                    preItem = item;
                    items.Add(item);
                }
            }
        }

        private void UpdateBottomToTop()
        {

        }

        private void UpdateLeftToRight()
        {

        }

        private void UpdateRightToLeft()
        {

        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!isDrag)
            {
                Debug.Log("[ScrollView] Start roll");
                isDrag = true;
                isRolling = true;
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            isDrag = false;
        }
    }

    public abstract class ScrollViewAdapter
    {
        protected ScrollView scrollView;

        protected Action<int, ScrollViewItem> onNewItem;

        protected ScrollViewPrefabData[] prefabDatas;
        protected float[] typeItemSizes;

        protected Stack<ScrollViewItem>[] caches;
        protected float[,] anchorPositions;

        protected float totalSize;
        protected int totalCount;

        protected bool initFinish;

        public ScrollViewAdapter(Action<int, ScrollViewItem> onNewItem)
        {
            this.onNewItem = onNewItem;
        }

        public void Init(ScrollView scrollView, ScrollViewPrefabData[] prefabDatas)
        {
            if (this.scrollView != null || this.prefabDatas != null)
            {
                Debug.LogWarning("[ScrollViewAdapter] can only init once!");
                return;
            }
            if (prefabDatas == null || prefabDatas.Length == 0)
            {
                throw new NullReferenceException("[ScrollViewAdapter] prefabs is null or empty!");
            }

            this.scrollView = scrollView;

            InitPrefabDatas(prefabDatas);
            CalculateTotalCount();

            CalculateTotalSize();

            initFinish = true;
        }

        private void InitPrefabDatas(ScrollViewPrefabData[] prefabDatas)
        {
            this.prefabDatas = prefabDatas;
            caches = new Stack<ScrollViewItem>[prefabDatas.Length];
            typeItemSizes = new float[prefabDatas.Length];
            for (int i = 0; i < prefabDatas.Length; ++i)
            {
                GameObject prefab = prefabDatas[i].prefab;

                if (prefab == null)
                {
                    throw new NullReferenceException();
                }

                caches[i] = new Stack<ScrollViewItem>();

                float size = 0;
                RectTransform rectTransform = prefab.GetComponent<RectTransform>();
                if (rectTransform != null)
                {
                    size = scrollView.IsVertical() ? rectTransform.rect.height : rectTransform.rect.width;
                }

                typeItemSizes[i] = size;
            }
        }

        private void CalculateTotalSize()
        {
            // reset totalSize
            totalSize = 0;

            if (typeItemSizes != null)
            {
                for (int i = 0; i < GetTypeCount(); ++i)
                {
                    totalSize += GetCountByType(i) * typeItemSizes[i];
                }
            }
        }

        public int GetCount()
        {
            return totalCount;
        }

        protected abstract void CalculateTotalCount();

        public float GetTotalSize()
        {
            return totalSize;
        }

        public int GetTypeCount()
        {
            return prefabDatas == null ? 0 : prefabDatas.Length;
        }

        private ScrollViewPrefabData GetPrefabDataByType(int type)
        {
            if (type < 0 || type >= GetTypeCount())
            {
                return null;
            }
            return prefabDatas[type];
        }

        private GameObject GetPrefabByType(int type)
        {
            ScrollViewPrefabData scrollViewPrefabData = GetPrefabDataByType(type);
            return scrollViewPrefabData == null ? null : scrollViewPrefabData.prefab;
        }

        protected abstract int GetCountByType(int type);

        protected abstract int GetTypeByIndex(int index);

        public float GetSizeOffset(int index)
        {
            int[] typeCount = new int[GetTypeCount()];

            for (int i = 0; i < index; ++i)
            {
                int type = GetTypeByIndex(index);
                if (type >= 0 && type < typeCount.Length)
                {
                    typeCount[type]++;
                }
            }

            float offset = 0;
            for (int i = 0; i < typeCount.Length; ++i)
            {
                offset += typeCount[i] * typeItemSizes[i];
            }
            return offset;
        }

        public ScrollViewItem CreateInstance(int index)
        {
            int type = GetTypeByIndex(index);

            ScrollViewItem item = null;
            if (caches != null && caches[type] != null && caches[type].Count > 0)
            {
                item = caches[type].Pop();
                item.gameObject.SetActive(true);
            }
            if (item == null)
            {
                ScrollViewPrefabData prefabData = GetPrefabDataByType(type);
                if (prefabData != null && prefabData.prefab != null)
                {
                    GameObject obj = GameObject.Instantiate(prefabData.prefab);
                    item = obj.AddComponent<ScrollViewItem>();

                    if (prefabData.widthStretch)
                    {
                        item.rectTransform.sizeDelta = new Vector2(scrollView.viewPortSzie.x, item.rectTransform.sizeDelta.y);
                    }

                    if (prefabData.heightStretch)
                    {
                        item.rectTransform.sizeDelta = new Vector2(item.rectTransform.sizeDelta.x, scrollView.viewPortSzie.y);
                    }
                    item.prefabType = type;
                }
            }

            if (item != null)
            {
                item.index = index;
                if (onNewItem != null)
                {
                    onNewItem.Invoke(index, item);
                }
            }

            return item;
        }

        public void RecycleInstance(ScrollViewItem item)
        {
            if (item == null)
            {
                return;
            }

            item.gameObject.SetActive(false);
            int type = item.prefabType;
            if (type < 0 && type >= GetTypeCount())
            {
                return;
            }
            if (caches[type] == null)
            {
                caches[type] = new Stack<ScrollViewItem>();

            }
            caches[type].Push(item);
        }
    }

    public class CustomAdapter : ScrollViewAdapter
    {
        private Verse[] datas;

        public CustomAdapter(Verse[] datas, Action<int, ScrollViewItem> onNewItem) : base(onNewItem)
        {
            this.datas = datas;
        }

        protected override void CalculateTotalCount()
        {
            totalCount = 0;

            if (datas != null)
            {
                for (int i = 0; i < datas.Length; ++i)
                {
                    ++totalCount;
                    totalCount += datas[i].verseContent == null ? 0 : datas[i].verseContent.Count;
                    if (i != datas.Length - 1)
                    {
                        ++totalCount;
                    }
                }
            }
        }

        protected override int GetCountByType(int type)
        {
            if (type < 0 || type >= GetTypeCount())
                return 0;
            if (type == 0)
            {
                return datas.Length;
            }
            if (type == 1)
            {
                int count = 0;
                for (int i = 0; i < datas.Length; ++i)
                {
                    count += datas[i].verseContent == null ? 0 : datas[i].verseContent.Count;
                }
                return count;
            }
            return datas.Length - 1;
        }

        protected override int GetTypeByIndex(int index)
        {
            int count = 0;
            for (int i = 0; i < datas.Length; ++i)
            {
                if (index == count++)
                {
                    return 0;
                }
                count += datas[i].verseContent == null ? 0 : datas[i].verseContent.Count;
                if (count > index)
                {
                    return 1;
                }
                ++count;
                if (count > index)
                {
                    return 2;
                }
            }
            return 2;
        }
    }

    [Serializable]
    public class ScrollViewPrefabData
    {
        public GameObject prefab;
        public bool widthStretch;
        public bool heightStretch;

        public string name
        {
            get
            {
                return prefab == null ? null : prefab.name;
            }
        }
    }

    public class Verse
    {
        public string title;
        public List<string> verseContent;

        public Verse(string title, List<string> verseContent)
        {
            this.title = title;
            this.verseContent = verseContent;
        }
    }
}
