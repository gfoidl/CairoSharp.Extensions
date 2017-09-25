using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace CreateKnownColors
{
    static class Program
    {
        private static XElement _xml;
        //---------------------------------------------------------------------
        static void Main()
        {
            _xml = XElement.Load("System.Drawing.Primitives.xml");

            using (StreamWriter sw = File.CreateText("KnownColors.cs"))
            {
                WriteHeader(sw);

                foreach (var color in GetColors().OrderBy(c => c.Color.Name))
                    WriteColor(sw, color.Color, color.Comment);

                WriteFooter(sw);
            }

            Console.WriteLine("Ende.");
            Console.ReadKey();
        }
        //---------------------------------------------------------------------
        private static IEnumerable<(Color Color, string Comment)> GetColors()
        {
            foreach (var item in typeof(Color).GetProperties(BindingFlags.Static | BindingFlags.Public))
            {
                if (item.PropertyType != typeof(Color)) continue;

                var color      = (Color)item.GetValue(null);
                string comment = GetComment(color);

                yield return (color, comment);
            }
        }
        //---------------------------------------------------------------------
        private static string GetComment(Color color)
        {
            XElement member = _xml
                .Descendants("member")
                .Where(x => x.Attribute("name").Value == $"P:System.Drawing.Color.{color.Name}")
                .FirstOrDefault();

            if (member == null) return null;

            string summary = member.Element("summary").Value;
            int index      = summary.IndexOf("ARGB");

            if (index < 0) return color.Name;

            string interestingPart = summary.Substring(index);

            return $"{color.Name} -- {interestingPart}";
        }
        //---------------------------------------------------------------------
        private static void WriteHeader(StreamWriter sw)
        {
            sw.WriteLine("using Cairo;");
            sw.WriteLine();
            sw.WriteLine("/* Created with CreateKnownColors. */");
            sw.WriteLine();
            sw.WriteLine("namespace CairoSharp.Extensions");
            sw.WriteLine("{");
            sw.WriteLine("/// <summary>");
            sw.WriteLine("/// Predefined colors.");
            sw.WriteLine("/// </summary>");
            sw.WriteLine("public static class KnownColors");
            sw.WriteLine("{");
        }
        //---------------------------------------------------------------------
        private static void WriteColor(StreamWriter sw, Color color, string comment = null)
        {
            double r = color.R / 255d;
            double g = color.G / 255d;
            double b = color.B / 255d;
            double a = color.A / 255d;

            string rr = r.ToString(CultureInfo.InvariantCulture);
            string gg = g.ToString(CultureInfo.InvariantCulture);
            string bb = b.ToString(CultureInfo.InvariantCulture);
            string aa = a.ToString(CultureInfo.InvariantCulture);

            sw.WriteLine("/// <summary>");
            sw.WriteLine("/// {0}", comment ?? color.Name);
            sw.WriteLine("/// </summary>");
            sw.WriteLine("public static readonly Color {0} = new Color({1}, {2}, {3}, {4});", color.Name, rr, gg, bb, aa);
            sw.WriteLine();
        }
        //---------------------------------------------------------------------
        private static void WriteFooter(StreamWriter sw)
        {
            sw.WriteLine("}");
            sw.WriteLine("}");
        }
    }
}