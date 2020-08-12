using System;
using System.Collections.Generic;
using System.Text;

namespace RedShot.Recording.Devices
{
    public class Device
    {
        public Device(string name) : this(name, name)
        {
        }

        public Device(string name, string compatibleFfmpegName)
        {
            Name = name;
            CompatibleFfmpegName = compatibleFfmpegName;
        }

        public string Name { get; }

        public string CompatibleFfmpegName { get; }
    }
}
