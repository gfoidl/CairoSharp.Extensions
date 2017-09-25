using System;
using Cairo;

namespace CairoSharp.Extensions.Arrows
{
    /// <summary>
    /// An arrow with a circle as head.
    /// </summary>
    public class CircleArrow : Arrow
    {
        /// <summary>
        /// Creates a <see cref="CircleArrow" />.
        /// </summary>
        /// <param name="radius">The radius of the arrow head's circle.</param>
        /// <remarks>
        /// The absolut value is used, no exception is thrown on negative values.
        /// </remarks>
        public CircleArrow(double radius = 0.5) : base(radius, 2 * Math.PI) { }
        //---------------------------------------------------------------------
        /// <summary>
        /// Draws the arrow head.
        /// </summary>
        /// <param name="context">The <see cref="Context" /></param>
        protected override void DrawArrowHead(Context context)
        {
            context.Arc(0, 0, _length, 0, 2 * Math.PI);
            context.Fill();
        }
    }
}