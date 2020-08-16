namespace RedShot.Helpers.Ffmpeg.Devices
{
    public class Device
    {
        public Device()
        {
        }

        public Device(string name) : this(name, name)
        {
        }

        public Device(string name, string compatibleFfmpegName)
        {
            Name = name;
            CompatibleFfmpegName = compatibleFfmpegName;
        }

        public string Name { get; set; } = string.Empty;

        public string CompatibleFfmpegName { get; set; } = string.Empty;

        public override string ToString()
        {
            return Name;
        }

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

        public override int GetHashCode()
        {
            return (Name + CompatibleFfmpegName).GetHashCode();
        }

        public bool Equals(Device device)
        {
            return Name == device.Name && CompatibleFfmpegName == device.CompatibleFfmpegName;
        }
    }
}
