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

            Content = new StackLayout()
            {
                Orientation = Orientation.Vertical,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                Width = 300,
                Padding = new Padding(10, 30),
                Spacing = 15,
                Items =
                {
                    questionLabel,
                    new StackLayout()
                    {
                        Orientation = Orientation.Horizontal,
                        VerticalContentAlignment = VerticalAlignment.Center,
                        Spacing = 15,
                        Items =
                        {
                            yesButton,
                            noButton
                        }
                    }
                }
            };

            yesButton.Clicked += YesButtonClicked;
            noButton.Clicked += NoButtonClicked;

            this.Shown += YesNoDialogShown;
        }

        private void YesNoDialogShown(object sender, System.EventArgs e)
        {
            Location = ScreenHelper.GetCenterLocation(Size);
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
            yesButton = new DefaultButton("Yes", 80, 40);
            noButton = new DefaultButton("No", 80, 40);
            questionLabel = new Label();
        }
    }
}
