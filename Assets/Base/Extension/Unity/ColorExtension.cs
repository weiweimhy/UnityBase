using UnityEngine;

namespace BaseFramework
{
#if UNITY_EDITOR
    using UnityEditor;

    public class ColorExtensionExample
    {
        [MenuItem("Example/Extension/Color")]
        public static void RunExample()
        {
            Log.I(typeof(ColorExtensionExample).Name, "#55675d = " + "55675d".ToColor());
            Log.I(typeof(ColorExtensionExample).Name, "#456 = " + "456".ToColor()); 
            Log.I(typeof(ColorExtensionExample).Name, "333 = " + "333".ToColor()); 
            Log.I(typeof(ColorExtensionExample).Name, "0xffb = " + "0xffb".ToColor()); 
            Log.I(typeof(ColorExtensionExample).Name, "#55555 = " + "55555".ToColor()); // parse error
        }
    }
#endif
    public static class ColorExtension
    {
        public static Color NewR(this Color self, float r)
        {
            return new Color(r, self.g, self.b, self.a);
        }

        public static Color NewG(this Color self, float g)
        {
            return new Color(self.r, g, self.b, self.a);
        }

        public static Color NewB(this Color self, float b)
        {
            return new Color(self.r, self.g, b, self.a);
        }

        public static Color NewA(this Color self, float a)
        {
            return new Color(self.r, self.g, self.b, a);
        }

        /// <summary>
        /// use in update
        /// </summary>
        /// <param name="self"></param>
        /// <param name="a"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static Color Fade(this Color self, float a, float t)
        {
            return self.NewA(Mathf.Lerp(self.a, a, t));
        }

        /// <summary>
        /// use in update
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static Color Tint(this Color from, Color to, float t)
        {
            return new Color(
                Mathf.Lerp(from.r, to.r, t),
                Mathf.Lerp(from.g, to.g, t),
                Mathf.Lerp(from.b, to.b, t),
                Mathf.Lerp(from.a, to.a, t));
        }

        /// <summary>
        /// Parse html color to unity color, return unity color, if parse error return Color(0,0,0,0)
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static Color ToColor(this string self, Color defaultVale = default(Color))
        {
            if (self.StartsWithIgnoreCase("0x"))
            {
                self = self.ReplaceIgnoreCase("0x", "#");
            }
            else if (!self.StartsWith("#"))
            {
                self = self.AddPrefix("#");
            }

            Color retColor;
            bool parseSucceed = ColorUtility.TryParseHtmlString(self, out retColor);
            if(!parseSucceed)
            {
                Log.W(typeof(ColorExtension), "{0} parse to color failed!", self);
                return defaultVale;
            }
            return retColor;
        }
    }
}