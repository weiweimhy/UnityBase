using UnityEngine;
using UnityEngine.UI;

namespace BaseFramework
{
    public static  class GraphicExtension
    {
        public static T ColorAlpha<T>(this T self, float alpha) where T : Graphic
        {
            if (self)
            {
                self.color = self.color.NewA(alpha);
            }
            return self;
        }

        public static T ColorRed<T>(this T self, float red) where T : Graphic
        {
            if (self)
            {
                self.color = self.color.NewR(red);
            }
            return self;
        }

        public static T ColorGreen<T>(this T self, float green) where T : Graphic
        {
            if(self)
            {
                self.color = self.color.NewG(green);
            }
            return self;
        }

        public static T ColorBlue<T>(this T self, float blue) where T : Graphic
        {
            if(self)
            {
                self.color = self.color.NewB(blue);
            }
            return self;
        }

        public static T Color<T>(this T self, Color color) where T:Graphic
        {
            if(self)
            {
                self.color = color;
            }
            return self;
        }
    }
}