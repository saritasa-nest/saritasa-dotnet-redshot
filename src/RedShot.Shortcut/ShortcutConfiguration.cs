using RedShot.Infrastructure.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace RedShot.Shortcut
{
    public class ShortcutConfiguration : IConfigurationOption
    {
        public string UniqueName => "Shortcut configuration";



        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
