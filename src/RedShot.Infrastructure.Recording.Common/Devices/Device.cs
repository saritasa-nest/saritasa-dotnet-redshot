using System;

namespace RedShot.Infrastructure.Recording.Common.Devices
{
    /// <summary>
    /// Recording device.
    /// </summary>
    public class Device : IEquatable<Device>
    {
        /// <summary>
        /// Initialize.
        /// </summary>
        public Device()
        {
        }

        /// <summary>
        /// Initialize with name.
        /// </summary>
        public Device(string name) : this(name, name)
        {
        }

        /// <summary>
        /// Initialize with name and compatible FFmpeg name.
        /// </summary>
        public Device(string name, string compatibleFfmpegName)
        {
            Name = name;
            CompatibleFfmpegName = compatibleFfmpegName;
        }

        /// <summary>
        /// Name.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Compatible FFmpeg name.
        /// </summary>
        public string CompatibleFfmpegName { get; set; } = string.Empty;

        /// <inheritdoc/>
        public override string ToString()
        {
            return Name;
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            if (obj is Device device)
            {
                return Equals(device);
            }
            else
            {
                return false;
            }
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return (Name + CompatibleFfmpegName).GetHashCode();
        }

        /// <inheritdoc/>
        public bool Equals(Device device)
        {
            return Name == device.Name && CompatibleFfmpegName == device.CompatibleFfmpegName;
        }
    }
}
