using System;
using Eto.Forms;

namespace RedShot.Infrastructure.Common.Forms
{
    public class DefaultButton : Panel
    {
        private readonly string name;
        private readonly int width;
        private readonly int height;

        private Button baseButton;

        public event EventHandler<EventArgs> Clicked;

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
