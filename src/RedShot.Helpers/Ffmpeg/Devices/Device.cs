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

        public string Name { get; set; }

        public string CompatibleFfmpegName { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
