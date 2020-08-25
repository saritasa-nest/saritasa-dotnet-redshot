using Eto.Forms;
using Eto.Drawing;
using System;
using RedShot.Infrastructure.Common;
using RedShot.Infrastructure.Abstractions;
using System.Threading.Tasks;
using System.Diagnostics;
using RedShot.Infrastructure.Uploading.Forms;
using System.Linq;

namespace RedShot.Infrastructure.Forms
{
    /// <summary>
    /// Upload bar form.
    /// </summary>
    public class UploadBar : Form
    {
        private IFile file;
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public UploadBar(IFile file)
        {
            this.file = file;
            InitializeComponent();
        }

        bool isFocused;
        bool canClose = true;
        bool blocked;
        UITimer checkTimer;
        UITimer closingTimer;
        UploadToolBar toolBar;

        void InitializeComponent()
        {
            BackgroundColor = Colors.FloralWhite;

            WindowStyle = WindowStyle.None;
            ShowInTaskbar = false;

            var size = ScreenHelper.GetMiniSizeDisplay();
            var sixteenpart = ScreenHelper.GetSixteenthPartOfDisplay();

            this.Location = ScreenHelper.GetStartPointForUploadView();

            checkTimer = new UITimer();
            checkTimer.Interval = 5;
            checkTimer.Elapsed += Timer_Elapsed;
            checkTimer.Start();

            closingTimer = new UITimer();
            closingTimer.Interval = 0.1;
            closingTimer.Elapsed += Timer_Elapsed1;

            Size = new Size(sixteenpart + size.Width, size.Height);

            var imageview = new ImageView();
            imageview.Image = file.GetFilePreview();
            imageview.Size = size;
            imageview.MouseDoubleClick += Imageview_MouseDoubleClick;

            toolBar = new UploadToolBar();

            Content = new StackLayout
            {
                Orientation = Orientation.Horizontal,
                Items =
                {
                    toolBar,
                    imageview
                }
            };
            Topmost = true;
            this.MouseMove += UploadBar_MouseMove;

            toolBar.CloseButton.Clicked += CloseButton_Clicked;
            toolBar.UploadersButton.Clicked += UploadersButton_Clicked;
        }

        private void UploadersButton_Clicked(object sender, EventArgs e)
        {
            RefreshTimer();

            var form = new UploaderChosingForm(file, UploadingManager.GetUploadingServices());
            form.Show();
        }

        private void CloseButton_Clicked(object sender, EventArgs e)
        {
            Close();
        }

        private void Imageview_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            OpenFile(file);
            RefreshTimer();
        }

        private void OpenFile(IFile file)
        {
            if (!string.IsNullOrEmpty(file.FilePath))
            {
                Task.Run(() =>
                {
                    try
                    {
                        Process.Start(
                            new ProcessStartInfo
                            {
                                FileName = file.FilePath,
                                UseShellExecute = true
                            });
                    }
                    catch (Exception e)
                    {
                        logger.Error(e, "An error occurred in opening image");
                    }
                });
            }
        }

        private void UploadBar_MouseMove(object sender, MouseEventArgs e)
        {
            RefreshTimer();
        }

        private void RefreshTimer()
        {
            isFocused = true;
            canClose = false;
            closingTimer.Stop();
            Opacity = 1;
        }

        private void Timer_Elapsed(object sender, EventArgs e)
        {
            if (blocked)
            {
                RefreshTimer();
                return;
            }
            else if (isFocused)
            {
                isFocused = false;
                return;
            }

            if (canClose == false)
            {
                closingTimer.Start();
                canClose = true;
            }
            else
            {
                Close();
            }
        }

        private void Timer_Elapsed1(object sender, EventArgs e)
        {
            Opacity = Opacity / 1.7;
        }
    }
}
