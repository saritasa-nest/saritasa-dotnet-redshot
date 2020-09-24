using RedShot.Infrastructure.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RedShot.Infrastructure.Uploading
{
    public class UploadingConfiguration : IConfigurationOption
    {
        public UploadingConfiguration()
        {
            UniqueName = "Uploading configuration";
            UploadersTypes = new List<Type>();
        }

        public string UniqueName { get; }

        public List<Type> UploadersTypes { get; set; }

        public bool AutoUpload { get; set; }

        public UploadingConfiguration Clone()
        {
            var clone = (UploadingConfiguration)MemberwiseClone();

            return clone;
        }

        object ICloneable.Clone()
        {
            return Clone();
        }
    }
}
