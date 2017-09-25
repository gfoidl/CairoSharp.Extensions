using System;
using Cairo;

namespace CairoSharp.Extensions.Shapes
{
    /// <summary>
    /// A circle shape.
    /// </summary>
    public sealed class Circle : Shape
    {
        private double _radius;
        //---------------------------------------------------------------------
        /// <summary>Creates a circle with given radius.</summary>
        /// <param name="radius">The radius.</param>
        /// <remarks>
        /// When <paramref name="radius" /> is negative, the absolute value
        /// gets used, no exception is thrown.
        /// </remarks>
        public Circle(double radius) => _radius = Math.Abs(radius);
        //---------------------------------------------------------------------
        /// <summary>
        /// Draws the shape.
        /// </summary>
        /// <param name="context">The <see cref="Context" /></param>
        /// <param name="x">The x-coordinate of the center.</param>
        /// <param name="y">The y-coordinate of the center.</param>
        protected override void MakePath(Context context, double x, double y)
        {
            // Translate to center:
            context.Save();
            context.Translate(x, y);
            {
#warning Check if this issue still is true
                // instead of radius the diameter was used by cairo
                context.Arc(0, 0, _radius / 2, 0, 2 * Math.PI);
            }
            context.Restore();
        }
    }
}