using System;
using System.Collections.Generic;
using Eto.Drawing;
using Eto.Forms;
using RedShot.Infrastructure.Common.Forms;
using RedShot.Infrastructure.Formatting.Formatters;

namespace RedShot.Infrastructure.Formatting.Settings
{
    /// <summary>
    /// Filename template dialog. Allows to design a filename template.
    /// </summary>
    public class FilenameTemplatePanel : Panel
    {
        private readonly IEnumerable<IFormatItem> formatItems;
        private readonly Action<string> addPattern;
        private ListBox formatsListBox;
        private Label exampleLabel;
        private Label patternLabel;
        private Label nameLabel;
        private DefaultButton addButton;

        /// <summary>
        /// Initializes <see cref="FilenameTemplatePanel"/> object.
        /// </summary>
        /// <param name="addPattern">Action that add new pattern to filename template textbox.</param>
        public FilenameTemplatePanel(Action<string> addPattern)
        {
            this.addPattern = addPattern;
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

            addButton = new DefaultButton("Add", 60, 25)
            {
                ToolTip = "Add format item"
            };
            addButton.Clicked += AddButtonClicked;

            Content = new StackLayout()
            {
                Orientation = Orientation.Vertical,
                HorizontalContentAlignment = HorizontalAlignment.Left,
                Padding = 15,
                Spacing = 15,
                Items =
                {
                    GetGuideFormatsStack()
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
                        HorizontalContentAlignment = HorizontalAlignment.Stretch,
                        Spacing = 10,
                        Items =
                        {
                            FormsHelper.GetBaseStack("Pattern:", patternLabel, 80, 100),
                            FormsHelper.GetBaseStack("Example:", exampleLabel, 80, 100),
                            addButton
                        }
                    }
                }
            };
        }

        private void AddButtonClicked(object sender, EventArgs e)
        {
            var selectedPattern = (IFormatItem) formatsListBox.SelectedValue;
            var fullPattern = GetFullPattern(selectedPattern.Pattern);
            addPattern(fullPattern);
        }

        private string GetFullPattern(string pattern) =>
            $"{FormatManager.FormatTag}{pattern}";
    }
}