using UnityEngine;
using UnityEngine.UI;

namespace BaseFramework.Test
{
    public class RecycleTestItem : MonoRecycleItem
    {

        private Image image;
        private Text text;

        private int _index;
        public int index
        {
            get
            {
                return _index;
            }
            set
            {
                _index = value;

                name = "RecycleTestItem_" + _index;
                text.text = _index.ToString();
            }
        }

        private Color _color;
        private Color color
        {
            get
            {
                return _color;
            }
            set
            {
                _color = value;
                image.color = _color;
            }
        }

        private void Awake()
        {
            image = GetComponent<Image>();
            text = transform.FindChild<Text>();
        }

        public override void Dispose()
        {
            this.Recycle();
        }
    }
}
