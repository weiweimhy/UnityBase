using UnityEngine;

namespace BaseFramework.UI
{
    public class ScrollViewItem : MonoBehaviour
    {
        public int prefabType;

        public int index;

        private RectTransform _rectTransform;
        public RectTransform rectTransform
        {
            get
            {
                if (_rectTransform == null)
                {
                    _rectTransform = GetComponent<RectTransform>();
                }
                return _rectTransform;
            }
        }
    }
}
