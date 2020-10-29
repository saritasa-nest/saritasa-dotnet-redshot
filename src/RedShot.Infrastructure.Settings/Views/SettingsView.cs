using System;
using System.Collections.Generic;
using Eto.Drawing;
using Eto.Forms;
using RedShot.Infrastructure.Common;
using RedShot.Infrastructure.Common.Forms;
using RedShot.Infrastructure.Settings.Sections;
using RedShot.Resources;

namespace RedShot.Infrastructure.Settings.Views
{
    /// <summary>
    /// Settings view.
    /// </summary>
    internal class SettingsView : Form
    {
        private readonly IEnumerable<ISettingsSection> settingsSections;

        private ListBox settingsListPanel;
        private Scrollable contentPanel;
        private DefaultButton okButton;
        private DefaultButton cancelButton;
        private bool saved;

        /// <summary>
        /// Initializes settings view.
        /// </summary>
        public SettingsView(IEnumerable<ISettingsSection> settingsSections)
        {
            Icon = new Icon(1, Icons.RedCircle);
            Title = "RedShot Settings";
            this.settingsSections = settingsSections;
            Resizable = false;
            Shown += SettingsView_Shown;
            MinimumSize = new Size(400, 400);
            Maximizable = false;

            InitializeComponents();
            this.Closing += SettingsView_Closing;
        }

        private void SettingsView_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (saved)
            {
                return;
            }

            var dialog = new YesNoDialog()
            {
                Message = "Do you want to close the settings without saving it?",
                Size = new Size(400, 200)
            };

            using (dialog)
            {
                if (dialog.ShowModal() != DialogResult.Yes)
                {
                    e.Cancel = true;
                }
            }
        }

        private void InitializeComponents()
        {
            settingsListPanel = GetSettingsListPanel();
            okButton = new DefaultButton("OK", 70, 25);
            okButton.Clicked += OkButton_Clicked;

            cancelButton = new DefaultButton("Cancel", 70, 25);
            cancelButton.Clicked += CancelButton_Clicked;

            Content = GetContentLayout();
        }

        private void CancelButton_Clicked(object sender, System.EventArgs e)
        {
            Close();
        }

        private void OkButton_Clicked(object sender, System.EventArgs e)
        {
            if (ValidateSettings())
            {
                foreach (var section in settingsSections)
                {
                    section.Save();
                }
                saved = true;
                Close();
            }
        }

        private bool ValidateSettings()
        {
            foreach (var section in settingsSections)
            {
                if (section is IValidatableSection validatable)
                {
                    var result = validatable.Validate();

                    if (!result.IsSuccess)
                    {
                        MessageBox.Show(this, result.ToString(), $"{section.Name} validation error", MessageBoxButtons.OK, MessageBoxType.Warning);
                        return false;
                    }
                }
            }

            return true;
        }

        private void SettingsView_Shown(object sender, System.EventArgs e)
        {
            Location = ScreenHelper.GetCenterLocation(Size);
        }

        private ListBox GetSettingsListPanel()
        {
            var listBox = new ListBox()
            {
                DataStore = settingsSections,
                Width = 180
            };

            listBox.ItemTextBinding = new DelegateBinding<ISettingsSection, string>(r => r.Name);
            listBox.SelectedValueChanged += ListBoxSelectedKeyChanged;

            return listBox;
        }

        private void ListBoxSelectedKeyChanged(object sender, EventArgs e)
        {
            if (settingsListPanel.SelectedValue is ISettingsSection section)
            {
                contentPanel.Content = section.GetControl();
            }
        }

        private Control GetContentLayout()
        {
            contentPanel = new Scrollable()
            {
                Size = new Size(800, 500)
            };

            var splitter = new Splitter
            {
                Position = 180,
                FixedPanel = SplitterFixedPanel.Panel2,
                Panel1 = settingsListPanel,
                Panel1MinimumSize = 180,
                Panel2MinimumSize = 800,
                Panel2 = contentPanel
            };

            return new StackLayout()
            {
                Orientation = Orientation.Vertical,
                HorizontalContentAlignment = HorizontalAlignment.Right,
                Padding = 10,
                Items =
                {
                    splitter,
                    new StackLayout()
                    {
                        Orientation = Orientation.Horizontal,
                        VerticalContentAlignment = VerticalAlignment.Center,
                        Padding = 10,
                        Spacing = 10,
                        Items =
                        {
                            okButton,
                            cancelButton
                        }
                    }
                }
            };
        }
    }
}
