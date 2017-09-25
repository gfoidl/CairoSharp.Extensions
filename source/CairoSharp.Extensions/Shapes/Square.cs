using System;
using Cairo;

namespace CairoSharp.Extensions.Shapes
{
    /// <summary>
    /// A square shape.
    /// </summary>
    public sealed class Square : Shape
    {
        private double _length;
        //---------------------------------------------------------------------
        /// <summary>Creates a square with given side length.</summary>
        /// <param name="length">The side lenght of the square.</param>
        /// <remarks>
        /// When <paramref name="length" /> is negative, the absolute value
        /// gets used, no exception is thrown.
        /// </remarks>
        public Square(double length) => _length = Math.Abs(length);
        //---------------------------------------------------------------------
        /// <summary>
        /// Draws the shape.
        /// </summary>
        /// <param name="context">The <see cref="Context" /></param>
        /// <param name="x">The x-coordinate of the center.</param>
        /// <param name="y">The y-coordinate of the center.</param>
        protected internal override void MakePath(Context context, double x, double y)
        {
            // Translate to center:
            context.Save();
            context.Translate(x, y);
            {
                context.Rectangle(-_length / 2, -_length / 2, _length, _length);
            }
            context.Restore();
        }
    }
}