using Cairo;

namespace CairoSharp.Extensions.Arrows
{
    /// <summary>
    /// A arrow or vector, where the arrow head is not filled (only the outline).
    /// </summary>
    public class OpenArrow : Arrow
    {
        /// <summary>
        /// Creates a <see cref="Arrow" />.
        /// </summary>
        /// <param name="length">Length of the arrow head.</param>
        /// <param name="angle">Half arrow head opening angle in degrees.</param>
        /// <remarks>
        /// The absolut values of <paramref name="length" /> and <paramref name="angle" />
        /// are used, no exception is thrown.
        /// </remarks>
        public OpenArrow(double length = 0.05, double angle = 10) : base(length, angle) { }
        //---------------------------------------------------------------------
        /// <summary>
        /// Draws the arrow head.
        /// </summary>
        /// <param name="context">The <see cref="Context" /></param>
        protected override void DrawArrowHead(Context context)
        {
            context.MoveTo(ArrowPoints[0]);
            context.LineTo(ArrowPoints[1]);
            context.MoveTo(ArrowPoints[0]);
            context.LineTo(ArrowPoints[2]);

            // Linie die sonst nicht dargestellt wird ergänzen:
            context.MoveTo(ArrowPoints[0]);
            context.LineTo(-_length, 0);

            context.Stroke();
        }
    }
}