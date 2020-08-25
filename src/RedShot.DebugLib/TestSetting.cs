using Eto.Forms;
using RedShot.Infrastructure.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace RedShot.DebugLib
{
    public class TestSetting : ISettingsOption
    {
        public string Name => "test";

        public Control GetControl()
        {
            return new StackLayout()
            {
                Items =
                {
                    new Button()
                }
            };
        }

        public void Save()
        {
            
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
