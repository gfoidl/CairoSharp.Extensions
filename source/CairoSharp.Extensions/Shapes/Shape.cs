using Cairo;

namespace CairoSharp.Extensions.Shapes
{
    /// <summary>
    /// A base implementation of a shape.
    /// </summary>
    public abstract class Shape
    {
        /// <summary>
        /// Draws the oultine of the shape.
        /// </summary>
        /// <param name="context">The <see cref="Context" /></param>
        /// <param name="centerX">The x-coordinate of the center.</param>
        /// <param name="centerY">The y-coordinate of the center.</param>
        public void Draw(Context context, double centerX, double centerY)
        {
            this.MakePath(context, centerX, centerY);
            context.Stroke();
        }
        //---------------------------------------------------------------------
        /// <summary>
        /// Draws the oultine of the shape with the given <see cref="Color" />.
        /// </summary>
        /// <param name="context">The <see cref="Context" /></param>
        /// <param name="centerX">The x-coordinate of the center.</param>
        /// <param name="centerY">The y-coordinate of the center.</param>
        /// <param name="color">
        /// The colour of the outline. Components must be given in the range [0,1].
        /// </param>
        public void Draw(Context context, double centerX, double centerY, Color color)
        {
            context.Color = color;
            this.Draw(context, centerX, centerY);
        }
        //---------------------------------------------------------------------
        /// <summary>
        /// Fills the shape.
        /// </summary>
        /// <param name="context">The <see cref="Context" /></param>
        /// <param name="centerX">The x-coordinate of the center.</param>
        /// <param name="centerY">The y-coordinate of the center.</param>
        public void Fill(Context context, double centerX, double centerY)
        {
            this.MakePath(context, centerX, centerY);
            context.Fill();
        }
        //---------------------------------------------------------------------
        /// <summary>
        /// Fills the shape with the given <see cref="Color" />.
        /// </summary>
        /// <param name="context">The <see cref="Context" /></param>
        /// <param name="centerX">The x-coordinate of the center.</param>
        /// <param name="centerY">The y-coordinate of the center.</param>
        /// <param name="color">
        /// The colour of the outline. Components must be given in the range [0,1].
        /// </param>
        public void Fill(Context context, double centerX, double centerY, Color color)
        {
            context.Color = color;
            this.Fill(context, centerX, centerY);
        }
        //---------------------------------------------------------------------
        /// <summary>
        /// Draws the shape.
        /// </summary>
        /// <param name="context">The <see cref="Context" /></param>
        /// <param name="x">The x-coordinate of the center.</param>
        /// <param name="y">The y-coordinate of the center.</param>
        protected abstract void MakePath(Context context, double x, double y);
    }
}