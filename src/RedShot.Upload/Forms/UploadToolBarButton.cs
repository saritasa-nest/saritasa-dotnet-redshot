using Eto.Forms;

namespace RedShot.Upload.Forms
{
    internal partial class UploadToolBarButton : Panel
    {
        private string name;

        public UploadToolBarButton(string name)
        {
            this.name = name;
            InitializeComponent();
        }
    }
}
