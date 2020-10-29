using Eto.Drawing;
using Eto.Forms;
using RedShot.Resources;

namespace RedShot.Infrastructure.Common.Forms
{
    /// <summary>
    /// Yes/No Dialog.
    /// </summary>
    public class YesNoDialog : Dialog<DialogResult>
    {
        /// <summary>
        /// Message (question).
        /// </summary>
        public string Message
        {
            get => questionLabel?.Text;
            set => questionLabel.Text = value;
        }

        private DefaultButton yesButton;
        private DefaultButton noButton;
        private Label questionLabel;

        /// <summary>
        /// Initializes yes/no dialog.
        /// </summary>
        public YesNoDialog()
        {
            Icon = new Icon(1, Icons.RedCircle);
            Title = "RedShot question";
            InitializeComponents();

            Content = new TableLayout
            {
                Padding = 15,
                Spacing = new Size(5, 5),
                Rows =
                {
                    new TableRow(new TableCell(new StackLayout(questionLabel))),
                    new TableRow(new StackLayout
                    {
                        Items =
                        {
                            new StackLayout()
                            {
                                Orientation = Orientation.Horizontal,
                                Padding = new Padding(0, 20, 0, 0),
                                Spacing = 5,
                                Items =
                                {
                                    yesButton,
                                    noButton
                                }
                            }
                        },
                        HorizontalContentAlignment = HorizontalAlignment.Right
                    })
                }
            };

            yesButton.Clicked += YesButtonClicked;
            noButton.Clicked += NoButtonClicked;

            this.Shown += YesNoDialogShown;
        }

        private void YesNoDialogShown(object sender, System.EventArgs e)
        {
            Location = ScreenHelper.GetCenterLocation(Size);
            Size = new Size(400, 135);
        }

        private void NoButtonClicked(object sender, System.EventArgs e)
        {
            Result = DialogResult.No;
            Close();
        }

        private void YesButtonClicked(object sender, System.EventArgs e)
        {
            Result = DialogResult.Yes;
            Close();
        }

        private void InitializeComponents()
        {
            yesButton = new DefaultButton("Yes", 70, 23);
            noButton = new DefaultButton("No", 70, 23);
            questionLabel = new Label();
        }
    }
}
