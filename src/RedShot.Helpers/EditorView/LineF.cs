using Eto.Drawing;

namespace RedShot.Helpers.EditorView
{
    /// <summary>
    /// Represents line between two points.
    /// </summary>
    public struct LineF
    {
        /// <summary>
        /// Inits Line.
        /// </summary>
        public LineF(PointF start, PointF end)
        {
            StartPoint = start;
            EndPoint = end;
        }

        /// <summary>
        /// Start point.
        /// </summary>
        public PointF StartPoint { get; set; }

        /// <summary>
        /// End point.
        /// </summary>
        public PointF EndPoint { get; set; }
    }
}
