using System.Collections.Generic;
using Eto.Drawing;
using Eto.Forms;
using RedShot.Infrastructure.Common;
using RedShot.Infrastructure.Common.Forms;
using RedShot.Infrastructure.Settings.Sections;

namespace RedShot.Infrastructure.Settings.Views
{
    /// <summary>
    /// Settings view.
    /// </summary>
    internal class SettingsView : Form
    {
        private readonly IEnumerable<ISettingsSection> settingsSections;

        private GridView nagivationPanel;
        private Scrollable contentPanel;
        private DefaultButton okButton;
        private DefaultButton cancelButton;

        /// <summary>
        /// Initializes settings view.
        /// </summary>
        public SettingsView(IEnumerable<ISettingsSection> settingsSections)
        {
            Title = "RedShot Settings";
            this.settingsSections = settingsSections;
            Resizable = false;
            Shown += SettingsView_Shown;
            MinimumSize = new Size(400, 400);

            InitializeComponents();
        }

        private void InitializeComponents()
        {
            nagivationPanel = GetNavigationPanel();
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

        private GridView GetNavigationPanel()
        {
            var gridView = new GridView()
            {
                AllowMultipleSelection = false,
                AllowColumnReordering = false,
                ShowHeader = false,
                AllowEmptySelection = false,
                DataStore = settingsSections,
                BackgroundColor = Colors.White
            };

            gridView.Columns.Add(new GridColumn
            {
                DataCell = new TextBoxCell
                {
                    Binding = new DelegateBinding<ISettingsSection, string>(r => r.Name)
                },
                Editable = false
            });

            gridView.CellClick += GridViewCellClick;

            return gridView;
        }

        private void GridViewCellClick(object sender, GridCellMouseEventArgs e)
        {
            if (e.Item is ISettingsSection section)
            {
                contentPanel.Content = section.GetControl();
            }
        }

        private Control GetContentLayout()
        {
            contentPanel = new Scrollable()
            {
                Size = new Size(500, 400)
            };

            var splitter = new Splitter
            {
                Position = 149,
                FixedPanel = SplitterFixedPanel.Panel1,
                Panel1 = nagivationPanel,
                Panel1MinimumSize = 150,
                Panel2MinimumSize = 400,
                Panel2 = contentPanel
            };

            return new StackLayout()
            {
                Orientation = Orientation.Vertical,
                HorizontalContentAlignment = HorizontalAlignment.Right,
                Padding = 20,
                Items =
                {
                    splitter,
                    new StackLayout()
                    {
                        Orientation = Orientation.Horizontal,
                        VerticalContentAlignment = VerticalAlignment.Center,
                        Padding = 10,
                        Items =
                        {
                            okButton,
                            FormsHelper.VoidRectangle(10, 1),
                            cancelButton
                        }
                    }
                }
            };
        }
    }
}
