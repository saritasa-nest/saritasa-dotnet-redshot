using Eto.Forms;
using System;
using System.Collections.Generic;
using System.Text;

namespace RedShot.Eto.Desktop.Forms.Common
{
    public abstract class NavigationForm : Form
    {
        public abstract event EventHandler<EventArgs> FormReadyToClose;

        public abstract event EventHandler<EventArgs> FormReadyToCancel;
    }
}
