using UnityEngine;
using UnityEngine.UI;

namespace BaseFramework
{
    public static class ImageExtension
    {
        public static Image FillAmount(this Image self, float fillamount)
        {
            if(self)
            {
                self.fillAmount = fillamount;
            }
            return self;
        }
    }
}