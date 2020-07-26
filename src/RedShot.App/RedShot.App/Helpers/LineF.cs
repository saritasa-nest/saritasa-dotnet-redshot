using Eto.Drawing;
using System;
using System.Collections.Generic;
using System.Text;

namespace RedShot.App.Helpers
{
    internal struct LineF
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
