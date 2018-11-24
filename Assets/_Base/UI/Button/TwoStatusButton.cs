using UnityEngine;
using UnityEngine.UI;

namespace BaseFramework
{
    [RequireComponent(typeof(Button))]
    public class TwoStatusButton : MonoBehaviour
    {
        public bool isActive = true;
        public Sprite inactiveSprite;

        private Sprite activeSprite;
        private Button button;

        protected virtual void Awake()
        {
            button = GetComponent<Button>();

            activeSprite = button.image.sprite;

            button.onClick.AddListener(() => {
                isActive = !isActive;
                button.image.sprite = isActive ? activeSprite : inactiveSprite;
            });
        }
    }
}
