using System;
using Eto.Forms;
using Eto.Drawing;
using RedShot.Infrastructure.Abstractions.Interfaces;
using RedShot.Eto.Desktop.Resources;

namespace RedShot.Eto.Desktop.Forms.Tray
{
    /// <summary>
    /// Tray icon for the application.
    /// </summary>
    public class ApplicationTray : Form
    {
        private readonly IMenuService menuService;
        private ButtonMenuItem uploadLastFileButton;

        /// <inheritdoc/>
        public TrayIndicator Tray { get; private set; }

        /// <summary>
        /// Initializes tray icon.
        /// </summary>
        public ApplicationTray(IMenuService menuService)
        {
            this.menuService = menuService;

            Title = "RedShot";
            InitializeComponents();
        }

        /// <summary>
        /// Initialize components.
        /// </summary>
        private void InitializeComponents()
        {
            uploadLastFileButton = new ButtonMenuItem()
            {
                Text = "Open last file",
                ToolTip = "Open last created file",
                Visible = false,
                Command = new Command((e, o) => OpenLastFile())
            };

            var menu = new ContextMenu();

            menu.Items.Add(new ButtonMenuItem()
            {
                Text = "Capture",
                ToolTip = "Take a screenshot",
                Command = new Command(async (e, o) => await menuService.TakeScreenshotAsync())
            });
            menu.Items.Add(new ButtonMenuItem()
            {
                Text = "Record",
                ToolTip = "Open view for video recording",
                //Command = new Command((e, o) => RecordingManager.Instance.InitiateRecording())
            });

            menu.Items.Add(uploadLastFileButton);

            menu.Items.Add(new SeparatorMenuItem());
            menu.Items.Add(new ButtonMenuItem()
            {
                Text = "Settings",
                //Command = new Command((e, o) => SettingsManager.OpenSettings())
            });
            menu.Items.Add(new ButtonMenuItem()
            {
                Text = "Send feedback",
                Command = new Command((e, o) => SendFeedBack())
            });

            menu.Items.Add(new SeparatorMenuItem());
            menu.Items.Add(new ButtonMenuItem()
            {
                Text = "Exit",
                Command = new Command((e, o) => Close())
            });

            Tray = new TrayIndicator
            {
                Menu = menu,
                Title = Title,
                Image = Icons.RedCircle
            };
        }

        /// <summary>
        /// Upload last file.
        /// </summary>
        private void OpenLastFile()
        {
            //var lastFile = UploadingProvider.LastFile;
            //if (lastFile != null)
            //{
            //    if (lastFile.FileType == FileType.Image)
            //    {
            //        var image = new Bitmap(lastFile.GetStream());
            //        ScreenshotManager.RunPaintingView(image);
            //    }
            //    else
            //    {
            //        var form = new UploadingForm(lastFile, UploadingProvider.GetUploadingServices());
            //        form.Show();
            //    }
            //}
        }

        /// <summary>
        /// Send feedback to specified email.
        /// </summary>
        private void SendFeedBack()
        {
            //var email = AppSettings.Instance.Email;
            //var url = $"mailto:{email}";

            //Process.Start(new ProcessStartInfo
            //{
            //    FileName = url,
            //    UseShellExecute = true
            //});
        }

        /// <inheritdoc/>
        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            Visible = false;
            Tray.Show();

            //StartLastFileCheckTimer();
        }

        /// <inheritdoc/>
        protected override void OnUnLoad(EventArgs e)
        {
            base.OnUnLoad(e);
            Tray.Hide();
        }

        //private void StartLastFileCheckTimer()
        //{
        //    var timer = new UITimer()
        //    {
        //        Interval = 1
        //    };

        //    timer.Elapsed += (o, e) =>
        //    {
        //        timer.Stop();
        //        if (UploadingProvider.LastFile != null)
        //        {
        //            uploadLastFileButton.Visible = true;
        //            return;
        //        }

        //        timer.Start();
        //    };

        //    timer.Start();
        //}
    }
}
