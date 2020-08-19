using System;
using Eto.Forms;

namespace RedShot.Infrastructure.Common.Forms
{
    /// <summary>
    /// Tool bar button.
    /// Will be improved.
    /// </summary>
    public partial class DefaultButton : Panel
    {
        public event EventHandler<EventArgs> Clicked;

        void InitializeComponent()
        {
            var btn = new Button();
            btn.Width = width;
            btn.Height = height;
            btn.Text = name;
            btn.Click += Btn_Click;

            Content = new StackLayout
            {
                Items =
                {
                    btn
                }
            };

        }

        private void Btn_Click(object sender, EventArgs e)
        {
            Clicked?.Invoke(this, EventArgs.Empty);
        }
    }
}
