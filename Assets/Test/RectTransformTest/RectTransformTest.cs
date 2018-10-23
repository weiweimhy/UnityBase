using UnityEngine;
using UnityEngine.UI;
using Base.Extension;
using Base.LogUtil;

namespace Base.Test
{
    public class RectTransformTest : MonoBehaviour
    {

        public Image whiteImage;
        public Image redImage;

        // Use this for initialization
        void Start()
        {
            Log.I(typeof(RectTransformTest), "White relative Red position:" 
                + whiteImage.rectTransform.RelativePosition(redImage.rectTransform));

            Log.I(typeof(RectTransformTest), "White Image Path:"
                + whiteImage.transform.GetPath());
        }

    }
}
