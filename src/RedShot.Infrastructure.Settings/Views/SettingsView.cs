using System;
using System.Collections.Generic;
using System.Linq;
using Eto.Drawing;
using Eto.Forms;
using RedShot.Infrastructure.Abstractions;
using RedShot.Infrastructure.Common.Forms;

namespace RedShot.Infrastructure.Settings.Views
{
    public class SettingsView : Form
    {
        private readonly IEnumerable<ISettingsOption> settingsOptions;
        private ListBox leftBar;
        private Control rightBar;
        private ISettingsOption selectedOption;

        public SettingsView(IEnumerable<ISettingsOption> settingsOptions)
        {
            this.settingsOptions = settingsOptions;
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            leftBar = GetLeftBar();
            rightBar = settingsOptions.First().GetControl();
            leftBar.SelectedValue = settingsOptions.First();

            UpdateContent();
        }

        private ListBox GetLeftBar()
        {
            var listBox = new ListBox()
            {
                Width = 200,
                Height = 580
            };
            listBox.DataStore = settingsOptions;
            listBox.SelectedValueChanged += ListBox_SelectedValueChanged;

            return listBox;
        }

        private void ListBox_SelectedValueChanged(object sender, EventArgs e)
        {
            if (selectedOption == null || !selectedOption.Equals(leftBar.SelectedValue))
            {
                selectedOption = (ISettingsOption)leftBar.SelectedValue;
                OpenOption(selectedOption);
            }
        }

        private void OpenOption(ISettingsOption option)
        {
            rightBar = option.GetControl();
            UpdateContent();
        }

        private void UpdateContent()
        {
            Content = new StackLayout()
            {
                Orientation = Orientation.Horizontal,
                VerticalContentAlignment = VerticalAlignment.Top,
                Padding = 10,
                Size = new Size(900, 600),
                Items =
                {
                    leftBar,
                    FormsHelper.VoidBox(10),
                    rightBar
                }
            };

            Invalidate(true);
        }
    }
}
