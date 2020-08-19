using System;
using System.Collections.Generic;
using System.Text;

namespace RedShot.Abstractions
{
    public interface IConfigurationManager
    {
        string ConfigurationFilePath { get; }

        void AddSection(IConfigurationSection section);

        IConfigurationSection GetSection(string uniqueName);

        void RemoveSection(string uniqueName);

        void SaveConfiguration();
    }
}
