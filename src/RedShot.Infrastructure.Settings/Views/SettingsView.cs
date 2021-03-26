using System;
using System.Collections.Generic;
using Eto.Drawing;
using Eto.Forms;
using RedShot.Resources;
using RedShot.Infrastructure.Abstractions.Settings;
using RedShot.Infrastructure.Common;
using RedShot.Infrastructure.Common.Forms;
using RedShot.Infrastructure.Configuration;
using RedShot.Infrastructure.Settings.Sections.Ftp;
using RedShot.Infrastructure.Settings.Sections.General;
using RedShot.Infrastructure.Settings.Sections.Recording;
using RedShot.Infrastructure.Settings.Sections.Shortcut;

namespace RedShot.Infrastructure.Settings.Views
{
    /// <summary>
    /// Settings view.
    /// </summary>
    internal class SettingsView : Form
    {
        private IReadOnlyCollection<ISettingsSection> settingsSections;

        private ListBox settingsListPanel;
        private Scrollable contentPanel;
        private DefaultButton okButton;
        private DefaultButton cancelButton;
        private bool saved;

        /// <summary>
        /// Initializes settings view.
        /// </summary>
        public SettingsView()
        {
            Icon = new Icon(1, Icons.RedCircle);
            Title = "RedShot Settings";
            Resizable = false;
            Shown += SettingsViewShown;
            Maximizable = false;

            RegisterSettingsSections();
            InitializeComponents();
            this.Closing += SettingsViewClosing;
            this.Closed += SettingsViewClosed;
        }

        private void SettingsViewClosed(object sender, EventArgs e)
        {
            foreach (var section in settingsSections)
            {
                section.Dispose();
            }
        }

        private void RegisterSettingsSections()
        {
            settingsSections = new List<ISettingsSection>
            {
                new RecordingSettingsSection(),
                new GeneralSettingsSection(),
                new ShortcutSettingsSection(),
                new FtpSettingsSection()
            };
        }

        private void SettingsViewClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (saved)
            {
                return;
            }

            const string message = "Do you want to close the settings without saving it?";
            const string title = "ResShot Question";
            var dialogResult = MessageBox.Show(message, title, MessageBoxButtons.YesNo,
                MessageBoxType.Question);

            if (dialogResult != DialogResult.Yes)
            {
                e.Cancel = true;
            }
        }

        private void InitializeComponents()
        {
            settingsListPanel = GetSettingsListPanel();
            okButton = new DefaultButton("OK", 70, 25);
            okButton.Clicked += OkButtonClicked;

            cancelButton = new DefaultButton("Cancel", 70, 25);
            cancelButton.Clicked += CancelButtonClicked;

            Content = GetContentLayout();
        }

        private void CancelButtonClicked(object sender, System.EventArgs e)
        {
            Close();
        }

        private void OkButtonClicked(object sender, System.EventArgs e)
        {
            if (ValidateSettings())
            {
                foreach (var section in settingsSections)
                {
                    section.Save();
                }
                ConfigurationProvider.Instance.Save();
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

        private void SettingsViewShown(object sender, System.EventArgs e)
        {
            Location = ScreenHelper.GetCenterLocation(Size);
        }

        private ListBox GetSettingsListPanel()
        {
            var listBox = new ListBox()
            {
                DataStore = settingsSections,
                Width = 170
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
                Size = new Size(500, 400),
            };

            var splitter = new Splitter
            {
                Position = 170,
                FixedPanel = SplitterFixedPanel.Panel2,
                Panel1 = settingsListPanel,
                Panel2 = contentPanel
            };

            return new StackLayout()
            {
                Orientation = Orientation.Vertical,
                HorizontalContentAlignment = HorizontalAlignment.Right,
                Padding = 5,
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
