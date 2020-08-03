using System;

namespace RedShot.Helpers.EditorView
{
    public static class ResizeHelper
    {
        public static bool ApproximatelyEquals(float a, float b)
        {
            return Math.Abs(a - b) < 5;
        }
    }
}
