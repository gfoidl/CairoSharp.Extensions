using Cairo;
using CairoSharp.Extensions.Shapes;

namespace CairoSharp.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="Context" />.
    /// </summary>
    public static class ContextExtensions
    {
        /// <summary>
        /// Draws a circle.
        /// </summary>
        /// <param name="context">The <see cref="Context" /></param>
        /// <param name="radius">The radius.</param>
        /// <param name="x">The x-coordinate of the center.</param>
        /// <param name="y">The y-coordinate of the center.</param>
        public static void Circle(this Context context, double radius, double x, double y)
        {
            var circle = new Circle(radius);
            circle.MakePath(context, x, y);
        }
        //---------------------------------------------------------------------
        /// <summary>
        /// Draws a square.
        /// </summary>
        /// <param name="context">The <see cref="Context" /></param>
        /// <param name="length">The side lenght of the square.</param>
        /// <param name="x">The x-coordinate of the center.</param>
        /// <param name="y">The y-coordinate of the center.</param>
        public static void Square(this Context context, double length, double x, double y)
        {
            var square = new Square(length);
            square.MakePath(context, x, y);
        }
        //---------------------------------------------------------------------
        /// <summary>
        /// Draws a hexagon.
        /// </summary>
        /// <param name="context">The <see cref="Context" /></param>
        /// <param name="cellSize">The cellsize (inner circle) of the hexagon.</param>
        /// <param name="x">The x-coordinate of the center.</param>
        /// <param name="y">The y-coordinate of the center.</param>
        public static void Hexagon(this Context context, double cellSize, double x, double y)
        {
            var hexagon = new Hexagon(cellSize);
            hexagon.MakePath(context, x, y);
        }
    }
}