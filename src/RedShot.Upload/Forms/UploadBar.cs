using Eto.Forms;
using Eto.Drawing;

namespace RedShot.Upload.Forms
{
    /// <summary>
    /// Upload bar form.
    /// </summary>
    public partial class UploadBar : Form
    {
        private Bitmap imageOnUpload;

        public UploadBar(Bitmap image)
        {
            imageOnUpload = image;
            InitializeComponent();
        }
    }
}
