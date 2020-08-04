using Eto.Forms;

namespace RedShot.Helpers.Forms
{
    public static class FormsHelper
    {
        public static Control VoidBox(int size)
        {
            return new StackLayout
            {
                VerticalContentAlignment = VerticalAlignment.Center,
                Orientation = Orientation.Horizontal,
                Size = new Eto.Drawing.Size(size, size),
                Padding = size
            };
        }
    }
}
