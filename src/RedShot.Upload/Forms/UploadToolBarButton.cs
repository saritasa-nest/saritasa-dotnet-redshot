using System;
using Eto.Forms;
using Eto.Drawing;

namespace RedShot.Upload.Forms
{
	public partial class UploadToolBarButton : Panel
	{
		private string name;
		public UploadToolBarButton(string name)
		{
			this.name = name;
			InitializeComponent();
		}
	}
}
