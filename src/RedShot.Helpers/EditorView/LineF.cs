using Eto.Drawing;

namespace RedShot.Helpers.EditorView
{
    public struct LineF
    {
        public LineF(PointF start, PointF end)
        {
            StartPoint = start;
            EndPoint = end;
        }

        public PointF StartPoint { get; set; }

        public PointF EndPoint { get; set; }
    }
}
