using Eto.Forms;

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
            get
            {
                return questionLabel?.Text;
            }

            set
            {
                questionLabel.Text = value;
            }
        }

        private DefaultButton yesButton;
        private DefaultButton noButton;
        private Label questionLabel;

        /// <summary>
        /// Initializes yes/no dialog.
        /// </summary>
        public YesNoDialog()
        {
            InitializeComponents();

            Content = new StackLayout()
            {
                Orientation = Orientation.Vertical,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                Padding = 20,
                Items =
                {
                    FormsHelper.VoidBox(20),
                    questionLabel,
                    FormsHelper.VoidBox(20),
                    new StackLayout()
                    {
                        Orientation = Orientation.Horizontal,
                        VerticalContentAlignment = VerticalAlignment.Center,
                        Height = 50,
                        Items =
                        {
                            yesButton,
                            FormsHelper.VoidRectangle(20, 1),
                            noButton
                        }
                    }
                }
            };

            yesButton.Clicked += YesButton_Clicked;
            noButton.Clicked += NoButton_Clicked;

            this.Shown += YesNoDialog_Shown;
        }

        private void YesNoDialog_Shown(object sender, System.EventArgs e)
        {
            Location = ScreenHelper.GetCenterLocation(Size);
        }

        private void NoButton_Clicked(object sender, System.EventArgs e)
        {
            Result = DialogResult.Cancel;
            Close();
        }

        private void YesButton_Clicked(object sender, System.EventArgs e)
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
