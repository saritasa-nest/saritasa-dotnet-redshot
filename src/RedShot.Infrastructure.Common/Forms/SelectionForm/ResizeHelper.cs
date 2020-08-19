using System;

namespace RedShot.Infrastructure.Common.Forms.SelectionForm
{
    /// <summary>
    /// Helper for resizing functions.
    /// </summary>
    public static class ResizeHelper
    {
        /// <summary>
        /// Checks if two value are approximately equal.
        /// </summary>
        public static bool ApproximatelyEquals(float a, float b)
        {
            return Math.Abs(a - b) < 5;
        }
    }
}
