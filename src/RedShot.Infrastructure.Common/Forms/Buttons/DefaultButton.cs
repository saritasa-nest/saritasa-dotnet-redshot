using System;
using Eto.Forms;

namespace RedShot.Infrastructure.Common.Forms
{
    /// <summary>
    /// Default button.
    /// Needs for correct work on different platforms.
    /// </summary>
    public class DefaultButton : Panel
    {
        private readonly string name;
        private readonly int width;
        private readonly int height;
        private Button baseButton;

        /// <summary>
        /// Event to handle when the user clicks the button.
        /// </summary>
        public event EventHandler<EventArgs> Clicked;

        /// <summary>
        /// Tool tip of the button.
        /// </summary>
        public override string ToolTip
        {
            get
            {
                return baseButton?.ToolTip;
            }

            set
            {
                baseButton.ToolTip = value;
            }
        }

        /// <summary>
        /// Initializes default button.
        /// </summary>
        public DefaultButton(string name, int width, int height)
        {
            this.name = name;
            this.width = width;
            this.height = height;

            InitializeComponent();
        }

        void InitializeComponent()
        {
            baseButton = new Button();
            baseButton.Width = width;
            baseButton.Height = height;
            baseButton.Text = name;
            baseButton.Click += Btn_Click;

            Content = new StackLayout
            {
                Items =
                {
                    baseButton
                }
            };
        }

        private void Btn_Click(object sender, EventArgs e)
        {
            Clicked?.Invoke(this, EventArgs.Empty);
        }
    }
}
