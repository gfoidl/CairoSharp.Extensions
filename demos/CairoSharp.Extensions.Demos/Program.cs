using System;
using System.IO;
using Cairo;
using CairoSharp.Extensions.Arrows;
using CairoSharp.Extensions.Shapes;
using IOPath = System.IO.Path;

namespace CairoSharp.Extensions.Demos
{
    static class Program
    {
        static void Main()
        {
            Directory.CreateDirectory("output");
            Environment.CurrentDirectory = IOPath.Combine(Environment.CurrentDirectory, "output");

            Arrows();
            Shapes();
            PaintAfter();
        }
        //---------------------------------------------------------------------
        private static void Arrows()
        {
            Action<Surface> draw = surface =>
            {
                using (var c = new Context(surface))
                {
                    c.Scale(300, 300);

                    // Is only relevant to PNG
                    c.Antialias = Antialias.Subpixel;

                    // adjust line width due scaling
                    double ux = 1, uy = 1;
                    c.DeviceToUserDistance(ref ux, ref uy);
                    c.LineWidth = Math.Max(ux, uy);

                    c.MoveTo(0, 0.1);
                    c.LineTo(1, 0.1);
                    c.MoveTo(0, 0.9);
                    c.LineTo(1, 0.9);
                    c.Stroke();

                    var arrow = new Arrow();
                    c.Color   = KnownColors.Blue;
                    arrow.DrawArrow(c, 0.1, 0.1, 0.2, 0.9);
                    arrow.DrawVector(c, 0.2, 0.1, 0.3, 0.9);

                    arrow   = new OpenArrow();
                    c.Color = new Color(0, 1, 0);
                    arrow.DrawArrow(c, 0.3, 0.1, 0.4, 0.9);
                    arrow.DrawVector(c, 0.4, 0.1, 0.5, 0.9);

                    arrow   = new CircleArrow(0.01);
                    c.Color = new Color(1, 0, 0);
                    arrow.DrawArrow(c, 0.5, 0.1, 0.6, 0.9);
                    arrow.DrawVector(c, 0.6, 0.1, 0.7, 0.9);
                }
            };

            using (Surface surface = new PdfSurface("arrows.pdf", 300, 300))
            {
                draw(surface);

                // PNG can also be created this way
                surface.WriteToPng("arrows.png");
            }

            using (Surface surface = new PSSurface("arrows.eps", 300, 300))
                draw(surface);

            using (Surface surface = new SvgSurface("arrows.svg", 300, 300))
                draw(surface);
        }
        //---------------------------------------------------------------------
        private static void Shapes()
        {
            Action<Surface> draw = surface =>
            {
                using (var c = new Context(surface))
                {
                    c.Antialias = Antialias.Subpixel;

                    // Hexagon:
                    Shape shape = new Hexagon(50);
                    shape.Draw(c, 50, 50);
                    shape.Fill(c, 50, 50, new Color(0.5, 0.5, 0.5));

                    // Square:
                    shape = new Square(50);
                    shape.Draw(c, 150, 50);
                    shape.Fill(c, 150, 50, new Color(0.5, 0.5, 0.5));

                    // Circle:
                    shape = new Circle(50);
                    shape.Draw(c, 100, 150);
                    shape.Fill(c, 100, 150, new Color(0.5, 0.5, 0.5));

                    // Bounding box:
                    var boundingBox = new Square(50);
                    c.LineWidth = 1;
                    var red = new Color(1, 0, 0);
                    boundingBox.Draw(c,  50,  50, red);
                    boundingBox.Draw(c, 150,  50, red);
                    boundingBox.Draw(c, 100, 150, red);
                }
            };

            using (Surface surface = new ImageSurface(Format.Argb32, 180, 180))
            {
                draw(surface);
                surface.WriteToPng("shapes.png");
            }

            using (Surface surface = new PdfSurface("shapes.pdf", 180, 180))
                draw(surface);

            using (Surface surface = new PSSurface("shapes.eps", 300, 300))
                draw(surface);

            using (Surface surface = new SvgSurface("shapes.svg", 180, 180))
                draw(surface);
        }
        //---------------------------------------------------------------------
        private static void PaintAfter()
        {
            Pattern pattern = null;

            using (Surface surface = new SvgSurface("test.svg", 300, 300))
            using (var c           = new Context(surface))
            {
                // Draw to group
                c.PushGroup();
                {
                    var hex = new Hexagon(150);
                    hex.Fill(c, 150, 150, new Color(0.8, 0.8, 0.8));
                }
                // Get group
                pattern = c.PopGroup();

                c.Source = pattern;

                // Draw (without mask, hence everything that's in the source)
                c.Paint();
            }

            using (Surface pdfSurface = new PdfSurface("test1.pdf", 300, 300))
            using (Surface svgSurface = new SvgSurface("test1.svg", 300, 300))
            using (var c = new Context(svgSurface))
            {
                c.PushGroup();
                {
                    c.Source = pattern;
                    c.Paint();
                    //pattern.Destroy();

                    c.Color = new Color(0, 0, 1);

                    c.LineWidth = 0.1;
                    c.MoveTo(0, 150);
                    c.LineTo(300, 150);
                    c.MoveTo(150, 0);
                    c.LineTo(150, 300);
                    c.Stroke();

                    c.Color = new Color(0, 0, 0);
                    //c.SelectFontFace("Georgia", FontSlant.Normal, FontWeight.Bold);
                    //c.SelectFontFace("Rockwell", FontSlant.Normal, FontWeight.Normal);
                    c.SelectFontFace("Arial", FontSlant.Normal, FontWeight.Normal);
                    //c.SelectFontFace("Times New Roman", FontSlant.Normal, FontWeight.Normal);
                    c.SetFontSize(16);
                    string text = "Hexagon with coordinate-axis";
                    //string text = "III";

                    // Determine the width of the text
                    double textWidth = c.GetTextWidth(text);

                    c.Save();
                    c.Translate((300 - textWidth) / 2, 16);
                    {
                        c.ShowText(text);
                    }
                    c.Restore();

                    c.MoveTo(0, 0);
                    c.LineTo(300, 300);
                    c.MoveTo(0, 300);
                    c.LineTo(300, 0);
                    c.Stroke();

                    c.Save();
                    c.Translate(16, 300);
                    c.Rotate(-Math.PI / 2);
                    c.Translate((300 - textWidth) / 2, 0);
                    {
                        c.ShowText(text);
                    }
                    c.Restore();

                    // Draw scala
                    c.Save();
                    c.Translate(270, 20);
                    {
                        // Farbverlauf:
                        Gradient linpat = new LinearGradient(0, 0, 0, 260);
                        linpat.AddColorStop(0, new Color(0, 1, 0));
                        linpat.AddColorStop(0.5, new Color(0, 0, 1));
                        linpat.AddColorStop(1, new Color(1, 0, 0));
                        c.Rectangle(0, 0, 20, 260);
                        c.Source = linpat;
                        //c.Fill();
                        c.FillPreserve();

                        c.Color = new Color(0, 0, 0);
                        c.LineWidth = 1;
                        //c.Rectangle(0, 0, 20, 260);
                        c.Stroke();
                    }
                    c.Restore();
                }
                pattern = c.PopGroup();
                c.Source = pattern;
                c.Paint();

                using (var c1 = new Context(pdfSurface))
                {
                    c1.Color = new Color(1, 1, 1, 0);
                    c1.Rectangle(0, 0, 300, 300);
                    c1.Fill();

                    c1.Source = pattern;
                    c1.Paint();

                    pdfSurface.WriteToPng("test1.png");
                }
            }

        }
    }
}