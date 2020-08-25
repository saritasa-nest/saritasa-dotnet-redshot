using RedShot.Infrastructure.Common.Forms;
using System;
using System.Collections.Generic;
using System.Text;

namespace RedShot.DebugLib
{
    public static class TestClass
    {
        public static void RunTest()
        {
            using (var yesNoDialog = new YesNoDialog())
            {
                yesNoDialog.Message = "FFmpeg is not installed. Do you want to automatically install it?";
                yesNoDialog.Title = "FFmpeg installing";
                yesNoDialog.ShowModal();
            }
        }
    }
}
