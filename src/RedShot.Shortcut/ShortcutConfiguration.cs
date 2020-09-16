using RedShot.Infrastructure.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace RedShot.Shortcut
{
    public class ShortcutConfiguration : IConfigurationOption
    {
        public string UniqueName => "Shortcut configuration";

        public List<ShortCutMap> ShortCutMaps { get; } = new List<ShortCutMap>();

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
