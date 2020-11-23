using System;
using Eto.Drawing;
using Eto.Forms;
using Prism.Events;
using RedShot.Infrastructure.Uploading.Uploaders.Ftp;
using RedShot.Infrastructure.Uploading.Uploaders.Ftp.Models;
using RedShot.Resources;

namespace RedShot.Infrastructure.Screenshooting.Painting
{
    /// <summary>
    /// Uploading button.
    /// </summary>
    internal class UploadingButton : Panel
    {
        private FtpAccount primaryAccount;

        /// <summary>
        /// Upload screen shot to FTP server event.
        /// </summary>
        public event EventHandler<DataEventArgs<FtpAccount>> UploadToFtpSelected;

        /// <summary>
        /// Upload screen shot to file event.
        /// </summary>
        public event EventHandler UploadToFileSelected;

        /// <summary>
        /// Upload screen shot to clipboard event.
        /// </summary>
        public event EventHandler UploadToClipboardSelected;

        /// <summary>
        /// Initializes image button.
        /// </summary>
        public UploadingButton()
        {
            var size = new Size(90, 30);
            primaryAccount = FtpAccountManager.GetPrimaryFtpAccount();

            var uploadButton = new SegmentedButton
            {
                Size = size,
                SelectionMode = SegmentedSelectionMode.Single
            };

            uploadButton.Items.Add(GetPrimaryUploadButton());
            uploadButton.Items.Add(new MenuSegmentedItem
            {
                Width = 30,
                Image = new Bitmap(Icons.DownArrow, 13, 12, ImageInterpolation.High),
                Menu = GetUploadButtonContextMenu()
            });

            Content = uploadButton;
        }

        private ButtonSegmentedItem GetPrimaryUploadButton()
        {
            var primaryUploadButton = new ButtonSegmentedItem()
            {
                Width = Convert.ToInt32(60),
                Image = new Bitmap(Icons.Upload, 20, 22, ImageInterpolation.High),
                ToolTip = "Upload"
            };

            if (primaryAccount == null)
            {
                primaryUploadButton.Enabled = false;
            }
            else
            {
                primaryUploadButton.Click += (o, e) =>
                {
                    primaryUploadButton.Selected = false;

                    UploadToFtpSelected?.Invoke(this, new DataEventArgs<FtpAccount>(primaryAccount));
                };
            }

            return primaryUploadButton;
        }

        private ContextMenu GetUploadButtonContextMenu()
        {
            var menu = new ContextMenu();

            var ftpAccounts = FtpAccountManager.GetFtpAccounts();

            foreach (var account in ftpAccounts)
            {
                var ftpItem = new ButtonMenuItem
                {
                    Text = account.ToString()
                };
                ftpItem.Click += (sender, e) => UploadToFtpSelected?.Invoke(this, new DataEventArgs<FtpAccount>(account));

                menu.Items.Add(ftpItem);
            }

            menu.Items.Add(new SeparatorMenuItem());

            var clipboardItem = new ButtonMenuItem
            {
                Text = "Clipboard",
                Shortcut = Keys.Control | Keys.C,
            };
            clipboardItem.Click += (sender, e) => UploadToClipboardSelected?.Invoke(this, EventArgs.Empty);

            var fileItem = new ButtonMenuItem
            {
                Text = "Save to file"
            };
            fileItem.Click += (sender, e) => UploadToFileSelected?.Invoke(this, EventArgs.Empty);

            menu.Items.Add(fileItem);
            menu.Items.Add(clipboardItem);

            return menu;
        }
    }
}
