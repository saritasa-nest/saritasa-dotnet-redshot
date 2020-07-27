using System;
using System.Collections.Generic;
using System.Text;

namespace RedShot.App.Helpers
{
    internal static class ResizeHelper
    {
        public static bool ApproximatelyEquals(float a, float b)
        {
            return Math.Abs(a - b) < 5;
        }
    }
}
