using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Cairo;

namespace CairoSharp.Extensions.KnownColorMap
{
    static class Program
    {
        static void Main()
        {
            const int rectWidth  = 200;
            const int rectHeight = 20;
            const int space      = 5;

            // sorted per HSV colorspace
            List<(Color Color, string Name)> colors = GetKnownColors()
                .Select(c => (c, c.Color.ToHSV()))
                .OrderBy(cc => cc.Item2[0])
                .ThenBy(cc => cc.Item2[1])
                .ThenBy(cc => cc.Item2[2])
                .Select(cc => cc.c)
                .ToList();

            const int surfaceWidth = rectWidth + 2 * space;
            int surfaceHeight      = (rectHeight + space) * (colors.Count + 1);

            using (var surface = new SvgSurface("known_colors.svg", surfaceWidth, surfaceHeight))
            using (var context = new Context(surface))
            {
                context.LineWidth = 0.35;
                context.SelectFontFace("Arial", FontSlant.Normal, FontWeight.Normal);
                context.SetFontSize(10);

                for (int i = 0; i < colors.Count; ++i)
                {
                    int y = 10 + i * (rectHeight + space);

                    context.Rectangle(10, y, rectWidth, rectHeight);
                    context.Color = colors[i].Color;
                    context.FillPreserve();

                    context.Color = new Color(0, 0, 0);
                    context.Stroke();

                    string name      = colors[i].Name;
                    double textWidth = context.GetTextWidth(name);

                    context.Color = colors[i].Color.GetInverseColor();
                    context.MoveTo((surfaceWidth - textWidth) / 2, y + context.FontExtents.Height);
                    context.ShowText(name);
                }
            }
        }
        //---------------------------------------------------------------------
        private static IEnumerable<(Color Color, string Name)> GetKnownColors()
        {
            var fields = typeof(KnownColors)
                .GetFields(BindingFlags.Static | BindingFlags.Public)
                .OrderBy(f => f.Name);

            foreach (FieldInfo field in fields)
            {
                Color color = (Color)field.GetValue(null);
                yield return (color, field.Name);
            }
        }
    }
}