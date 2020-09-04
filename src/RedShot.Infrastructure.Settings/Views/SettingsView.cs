using System.Collections.Generic;
using Eto.Forms;
using RedShot.Infrastructure.Abstractions;
using RedShot.Infrastructure.Common;
using RedShot.Infrastructure.Common.Forms;

namespace RedShot.Infrastructure.Settings.Views
{
    /// <summary>
    /// Settings view.
    /// </summary>
    internal class SettingsView : Form
    {
        private readonly IEnumerable<ISettingsOption> settingsOptions;

        /// <summary>
        /// Initializes settings view.
        /// </summary>
        public SettingsView(IEnumerable<ISettingsOption> settingsOptions)
        {
            Title = "RedShot Settings";
            this.settingsOptions = settingsOptions;
            Content = GetContentLayout();
            Resizable = false;
            Shown += SettingsView_Shown;
        }

        private void SettingsView_Shown(object sender, System.EventArgs e)
        {
            Location = ScreenHelper.GetCenterLocation(Size);
        }

        private Control GetContentLayout()
        {
            var layout = new StackLayout()
            {
                Orientation = Orientation.Vertical,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                Padding = 20
            };

            layout.Items.Add(FormsHelper.VoidBox(30));

            foreach (var option in settingsOptions)
            {
                layout.Items.Add(GetOptionButton(option));
                layout.Items.Add(FormsHelper.VoidBox(30));
            }

            return layout;
        }

        private Control GetOptionButton(ISettingsOption option)
        {
            var button = new DefaultButton(option.Name, 250, 40);
            button.Clicked += (o, e) =>
            {
                var dialog = option.GetOptionDialog();
                if (dialog != null && dialog.ShowModal(this) == DialogResult.Ok)
                {
                    option.Save();
                }
            };

            return button;
        }
    }
}
