using System;
using Cairo;

namespace CairoSharp.Extensions.Arrows
{
    /// <summary>
    /// A arrow or vector.
    /// </summary>
    public class Arrow
    {
        /// <summary>
        /// Length of arrow head.
        /// </summary>
        protected double _length;

        /// <summary>
        /// Half arrow head opening angle in radiants.
        /// </summary>
        protected double _angle;
        //---------------------------------------------------------------------
        private PointD[] _arrowPoints;
        /// <summary>
        /// The <see cref="PointD"/>s constructing the arrow head.
        /// </summary>
        /// <remarks>
        /// The head is at (0,0) and the ends at (-l, l.tan(a)) respective (-l, -l.tan(a)).
        /// Therefore the points have to be brought by transformation to the correct place.
        /// </remarks>
        protected PointD[] ArrowPoints
        {
            get
            {
                if (_arrowPoints == null)
                {
                    double x1 = -_length;
                    double y1 = _length * Math.Tan(_angle);

                    _arrowPoints = new PointD[]
                    {
                        new PointD(0, 0),
                        new PointD(x1, y1),
                        new PointD(x1, -y1)
                    };
                }

                return _arrowPoints;
            }
        }
        //---------------------------------------------------------------------
        /// <summary>
        /// Creates a <see cref="Arrow" />.
        /// </summary>
        /// <param name="length">Length of the arrow head.</param>
        /// <param name="angle">Half arrow head opening angle in degrees.</param>
        /// <remarks>
        /// The absolut values of <paramref name="length" /> and <paramref name="angle" />
        /// are used, no exception is thrown.
        /// </remarks>
        public Arrow(double length = 0.05, double angle = 10)
        {
            _length = Math.Abs(length);
            _angle  = Math.Abs(angle) * Math.PI / 180d;     // Grad -> Radianten
        }
        //---------------------------------------------------------------------
        /// <summary>
        /// Draws a vector, i.e. an arrow with arrow head.
        /// </summary>
        /// <param name="context">The <see cref="Context" /></param>
        /// <param name="x0">The x-coordinate of the start-point.</param>
        /// <param name="y0">The y-coordinate of the start-point.</param>
        /// <param name="x1">The x-coordinate of the end-point.</param>
        /// <param name="y1">The y-coordinate of the end-point.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="context" /> is <c>null</c>.
        /// </exception>
        public void DrawVector(Context context, double x0, double y0, double x1, double y1)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            this.DrawVector(context, new PointD(x0, y0), new PointD(x1, y1));
        }
        //---------------------------------------------------------------------
        /// <summary>
        /// Draws a vector, i.e. an arrow with arrow head.
        /// </summary>
        /// <param name="context">The <see cref="Context" /></param>
        /// <param name="start">Start-point.</param>
        /// <param name="end">End-point.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="context" /> is <c>null</c>.
        /// </exception>
        public void DrawVector(Context context, PointD start, PointD end)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            this.Draw(context, start, end, false);
        }
        //---------------------------------------------------------------------
        /// <summary>
        /// Draws an arrow.
        /// </summary>
        /// <param name="context">The <see cref="Context" /></param>
        /// <param name="x0">The x-coordinate of the start-point.</param>
        /// <param name="y0">The y-coordinate of the start-point.</param>
        /// <param name="x1">The x-coordinate of the end-point.</param>
        /// <param name="y1">The y-coordinate of the end-point.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="context" /> is <c>null</c>.
        /// </exception>
        public void DrawArrow(Context context, double x0, double y0, double x1, double y1)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            this.DrawArrow(context, new PointD(x0, y0), new PointD(x1, y1));
        }
        //---------------------------------------------------------------------
        /// <summary>
        /// Draws an arrow.
        /// </summary>
        /// <param name="context">The <see cref="Context" /></param>
        /// <param name="start">Start-point.</param>
        /// <param name="end">End-point.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="context" /> is <c>null</c>.
        /// </exception>
        public void DrawArrow(Context context, PointD start, PointD end)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            this.Draw(context, start, end, true);
        }
        //---------------------------------------------------------------------
        private void Draw(Context c, PointD start, PointD end, bool drawHeadOnStart)
        {
            double dx     = end.X - start.X;
            double dy     = end.Y - start.Y;
            double angle  = Math.Atan2(dy, dx);
            double length = Math.Sqrt(dx * dx + dy * dy);

            // Zum Startpunkt verschieben und so rotieren dass die x-Achse
            // mit dem Vektor koinzidiert. Hierbei ausnutzen dass Math.Atan2
            // mathematisch positiv ist und die Rotate-Methode mathematisch
            // negativ.
            c.Save();
            c.Translate(start.X, start.Y);
            c.Rotate(angle);
            {
                // Die Linie zeichnen. Dabei berücksichtigen dass die Linie nur
                // bis zum Beginn der Spitze gezeichnet wird.
                if (drawHeadOnStart)
                    c.MoveTo(_length, 0);
                else
                    c.MoveTo(0, 0);

                c.LineTo(length - _length, 0);
                c.Stroke();

                // Pfeilspitzen zeichnen.
                // Start:
                if (drawHeadOnStart)
                {
                    // Um 180° rotieren:
                    c.Save();
                    c.Rotate(Math.PI);
                    {
                        DrawArrowHead(c);
                    }
                    c.Restore();
                }

                // Ende - dazu zum Endpunkt verschieben:
                c.Save();
                c.Translate(length, 0);
                {
                    DrawArrowHead(c);
                }
                c.Restore();
            }
            c.Restore();
        }
        //---------------------------------------------------------------------
        /// <summary>
        /// Draws the arrow head.
        /// </summary>
        /// <param name="context">The <see cref="Context" /></param>
        protected virtual void DrawArrowHead(Context context)
        {
            // Pfad der Pfeilspitze zeichnen:
            context.MoveTo(this.ArrowPoints[0]);
            context.LineTo(this.ArrowPoints[1]);
            context.LineTo(this.ArrowPoints[2]);
            context.ClosePath();

            // Auf das Papier bringen:
            context.Fill();
        }
    }
}