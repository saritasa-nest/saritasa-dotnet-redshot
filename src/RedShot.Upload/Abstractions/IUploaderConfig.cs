using System;
using System.Collections.Generic;
using System.Text;

namespace RedShot.Upload.Abstractions
{
    public interface IUploaderConfig
    {
        string SectionName { get; }
    }
}
