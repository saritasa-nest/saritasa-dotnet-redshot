using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using RedShot.Infrastructure.Recording.Ffmpeg.Devices;

namespace RedShot.Infrastructure.Recording.Ffmpeg
{
    /// <summary>
    /// Audio options.
    /// </summary>
    public class AudioOptions : INotifyPropertyChanged, ICloneable
    {
        private bool recordAudio;

        /// <summary>
        /// Constructor.
        /// </summary>
        public AudioOptions()
        {
            Devices = new HashSet<Device>();
        }

        /// <summary>
        /// Record audio flag.
        /// </summary>
        public bool RecordAudio
        {
            get
            {
                return recordAudio;
            }

            set
            {
                recordAudio = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Record audio devices.
        /// </summary>
        public HashSet<Device> Devices { get; set; }

        /// <inheritdoc/>
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string memberName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
        }

        /// <summary>
        /// Clone.
        /// </summary>
        public AudioOptions Clone()
        {
            return MemberwiseClone() as AudioOptions;
        }

        object ICloneable.Clone()
        {
            return Clone();
        }
    }
}
