using System;
using System.Collections.Generic;
using Eto.Drawing;
using Eto.Forms;
using RedShot.Infrastructure.Common.Forms;
using RedShot.Infrastructure.Formatting.Formatters;

namespace RedShot.Infrastructure.Formatting.Settings
{
    /// <summary>
    /// Format guide dialog.
    /// Guides how an user can format file link.
    /// </summary>
    public class FormatGuidePanel : Panel
    {
        private readonly IEnumerable<IFormatItem> formatItems;
        private ListBox formatsListBox;
        private Label exampleLabel;
        private Label patternLabel;
        private Label nameLabel;

        /// <summary>
        /// Initializes format guide dialog.
        /// </summary>
        public FormatGuidePanel()
        {
            formatItems = FormatManager.FormatItems;
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            exampleLabel = new Label()
            {
                Width = 100
            };
            patternLabel = new Label()
            {
                Width = 100
            };
            nameLabel = new Label()
            {
                Width = 100
            };

            formatsListBox = new ListBox()
            {
                DataStore = formatItems,
                ItemTextBinding = new PropertyBinding<string>("Name"),
                AllowDrop = true,
                Size = new Size(150, 100)
            };
            formatsListBox.SelectedValueChanged += FormatsListBoxSelectedValueChanged;

            Content = new StackLayout()
            {
                Orientation = Orientation.Vertical,
                HorizontalContentAlignment = HorizontalAlignment.Left,
                Padding = 15,
                Spacing = 15,
                Items =
                {
                    GetGuideFormatsStack(),
                    GetUsersTextFormatGuide()
                }
            };
        }

        private void FormatsListBoxSelectedValueChanged(object sender, EventArgs e)
        {
            if (formatsListBox.SelectedValue is IFormatItem item)
            {
                exampleLabel.Text = item.GetText();
                patternLabel.Text = $"%{item.Pattern}";
                nameLabel.Text = item.Name;
            }
        }

        private Control GetGuideFormatsStack()
        {
            return new StackLayout()
            {
                Orientation = Orientation.Horizontal,
                VerticalContentAlignment = VerticalAlignment.Center,
                Spacing = 15,
                Items =
                {
                    formatsListBox,
                    new StackLayout()
                    {
                        Orientation = Orientation.Vertical,
                        HorizontalContentAlignment = HorizontalAlignment.Left,
                        Spacing = 10,
                        Items =
                        {
                            FormsHelper.GetBaseStack("Name:", nameLabel, 80, 250),
                            FormsHelper.GetBaseStack("Pattern:", patternLabel, 80, 250),
                            FormsHelper.GetBaseStack("Example:", exampleLabel, 80, 250),
                        }
                    }
                }
            };
        }

        private Control GetUsersTextFormatGuide()
        {
            return new StackLayout()
            {
                Orientation = Orientation.Horizontal,
                VerticalContentAlignment = VerticalAlignment.Center,
                Spacing = 10,
                Items =
                {
                    new Label()
                    {
                        Text = "If you want to insert your own text:"
                    },
                    new Label()
                    {
                        Text = "%[your_text]"
                    }
                }
            };
        }
    }
}