using System;
using Cairo;

namespace CairoSharp.Extensions.Shapes
{
    /// <summary>
    /// A hexagon shape.
    /// </summary>
    public sealed class Hexagon : Shape
    {
        private static readonly double sqrt3Inv = 1d / Math.Sqrt(3d);
        private double _cellSize;
        //---------------------------------------------------------------------
        // Caching of hexan points, not necessary to redetermine on each access.
        private PointD[] _hexagonPoints;
        private PointD[] HexagonPoints => _hexagonPoints = _hexagonPoints ?? this.GetHexagonPoints();
        //---------------------------------------------------------------------
        /// <summary>
        /// Creates a hexagon with given cellsize (inner circle).
        /// </summary>
        /// <param name="cellSize">The cellsize (inner circle) of the hexagon.</param>
        /// <remarks>
        /// When <paramref name="cellSize" /> is negative, the absolute value
        /// gets used, no exception is thrown.
        /// </remarks>
        public Hexagon(double cellSize) => _cellSize = cellSize;
        //---------------------------------------------------------------------
        /// <summary>
        /// Draws the shape.
        /// </summary>
        /// <param name="context">The <see cref="Context" /></param>
        /// <param name="x">The x-coordinate of the center.</param>
        /// <param name="y">The y-coordinate of the center.</param>
        protected override internal void MakePath(Context context, double x, double y)
        {
            // Translate to center:
            context.Save();
            context.Translate(x, y);
            {
                context.MoveTo(this.HexagonPoints[0]);
                context.LineTo(this.HexagonPoints[1]);
                context.LineTo(this.HexagonPoints[2]);
                context.LineTo(this.HexagonPoints[3]);
                context.LineTo(this.HexagonPoints[4]);
                context.LineTo(this.HexagonPoints[5]);
                context.ClosePath();
            }
            context.Restore();
        }
        //---------------------------------------------------------------------
        private PointD[] GetHexagonPoints()
        {
            // Anpassen der Seitenlänge damit der Abstand zwischen 2 Sechsecken
            // 2.ri beträgt.
            // Herleitung über: 2.ri = a.sqrt(3), 
            // ri...Inkreisradius, a=r...Kantenlänge
            double ri = _cellSize / 2;
            double r  = 2 * ri * sqrt3Inv;

            // Punkte für das Sechseck:
            var p1 = new PointD(0  , r);
            var p2 = new PointD(ri , r / 2);
            var p3 = new PointD(ri , -r / 2);
            var p4 = new PointD(0  , -r);
            var p5 = new PointD(-ri, -r / 2);
            var p6 = new PointD(-ri, r / 2);

            return new PointD[] { p1, p2, p3, p4, p5, p6 };
        }
    }
}