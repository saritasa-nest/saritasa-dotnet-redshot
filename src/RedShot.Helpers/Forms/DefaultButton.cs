using Eto.Forms;

namespace RedShot.Helpers.Forms
{
    public partial class DefaultButton : Panel
    {
        private readonly string name;
        private readonly int width;
        private readonly int height;

        public DefaultButton(string name, int width, int height)
        {
            this.name = name;
            this.width = width;
            this.height = height;

            InitializeComponent();
        }
    }
}
