using System;
using System.Collections.Generic;
using System.Linq;
using Eto.Drawing;
using Eto.Forms;
using RedShot.Infrastructure.Common.Forms;
using RedShot.Infrastructure.Formatting;
using RedShot.Infrastructure.Formatting.Formatters;

namespace RedShot.Infrastructure.Settings.Sections.General
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
            formatItems = MoveCustomItemToEnd(FormatManager.FormatItems);
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
            formatsListBox.SelectedValueBinding.Convert(sv => sv != null)
                .Bind(addButton, b => b.Enabled, DualBindingMode.OneWayToSource);

            Content = GetFilenameTemplateStack();
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

        private Control GetFilenameTemplateStack()
        {
            return new StackLayout()
            {
                Orientation = Orientation.Horizontal,
                VerticalContentAlignment = VerticalAlignment.Top,
                Spacing = 15,
                Padding = 10,
                Items =
                {
                    formatsListBox,
                    new StackLayout()
                    {
                        Orientation = Orientation.Vertical,
                        HorizontalContentAlignment = HorizontalAlignment.Stretch,
                        Spacing = 5,
                        Items =
                        {
                            FormsHelper.GetBaseStack("Pattern:", patternLabel, 40, 100, 2),
                            FormsHelper.GetBaseStack("Example:", exampleLabel, 47, 100, 2),
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

        private IEnumerable<IFormatItem> MoveCustomItemToEnd(IEnumerable<IFormatItem> formatItems)
        {
            var formatItemsList = formatItems.ToList();
            var customItem = formatItemsList.First(it => it is CustomFormatItem);
            formatItemsList.Remove(customItem);
            return formatItemsList.Append(customItem);
        }
    }
}