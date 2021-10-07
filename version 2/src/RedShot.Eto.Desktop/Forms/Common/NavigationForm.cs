using Eto.Forms;
using RedShot.Eto.Mvp.Presenters.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace RedShot.Eto.Desktop.Forms.Common
{
    public abstract class NavigationForm : Form, IView
    {
        public event EventHandler<EventArgs> FormReadyToClose;

        public new void Close()
        {
            throw new InvalidOperationException("You cannot close the form from its inside.");
        }

        protected void CallReadyToClose()
        {
            FormReadyToClose?.Invoke(this, EventArgs.Empty);
        }

        void IView.Close()
        {
            base.Close();
        }
    }
}
