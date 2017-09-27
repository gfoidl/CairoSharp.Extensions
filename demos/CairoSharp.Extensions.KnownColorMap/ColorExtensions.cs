using System;
using Cairo;

namespace CairoSharp.Extensions.KnownColorMap
{
    internal static class ColorExtensions
    {
        public static double[] ToHSV(this Color color)
        {
            double r = color.R;
            double g = color.G;
            double b = color.B;

            double max = Math.Max(r, Math.Max(g, b));
            double min = Math.Min(r, Math.Min(g, b));

            double h = 0;
            if (max == r)
            {
                if (g > b)
                    h = 60 * (g - b) / (max - min);
                else if (g < b)
                    h = 60 * (g - b) / (max - min) + 360;
            }
            else if (max == g)
                h = 60 * (b - r) / (max - min) + 120;
            else if (max == b)
                h = 60 * (r - g) / (max - min) + 240;

            double s = (max == 0) ? 0 : 1 - min / max;

            return new double[] { h, s, max };
        }
        //---------------------------------------------------------------------
        public static Color GetInverseColor(this Color color)
        {
            if (color == KnownColors.Gray) return new Color(1, 1, 1);

            return new Color(1 - color.R, 1 - color.G, 1 - color.B);
        }
    }
}