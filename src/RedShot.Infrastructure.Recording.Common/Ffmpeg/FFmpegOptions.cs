using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using RedShot.Infrastructure.Common.Recording.Encoding;
using RedShot.Infrastructure.Recording.Common.Devices;

namespace RedShot.Infrastructure.Recording.Common.Ffmpeg
{
    /// <summary>
    /// FFmpeg options.
    /// </summary>
    public class FFmpegOptions : INotifyPropertyChanged, ICloneable
    {
        private int fps;
        private bool drawCursor;
        private Device videoDevice;
        private FFmpegVideoCodec videoCodec;
        private FFmpegAudioCodec audioCodec;
        private FFmpegX264Preset x264Preset;
        private int x264Crf;
        private int vp9Crf;
        private int xviDQscale;
        private int vp9Bitrate;
        private int aacQScale;
        private int opusBitrate;
        private int vorbisQscale;
        private int mp3Qscale;
        private string userArgs;
        private bool useGdigrab;

        /// <summary>
        /// FPS.
        /// </summary>
        public int Fps
        {
            get { return fps; }

            set
            {
                if (fps != value)
                {
                    if (value > 14 && value < 61)
                    {
                        fps = value;
                        OnPropertyChanged();
                    }
                }
            }
        }

        /// <summary>
        /// Draw cursor.
        /// </summary>
        public bool DrawCursor
        {
            get { return drawCursor; }

            set
            {
                if (drawCursor != value)
                {
                    drawCursor = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Use GDI grab.
        /// Windows only.
        /// </summary>
        public bool UseGdigrab
        {
            get { return useGdigrab; }

            set
            {
                if (useGdigrab != value)
                {
                    useGdigrab = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Video device.
        /// </summary>
        public Device VideoDevice
        {
            get { return videoDevice; }

            set
            {
                if (videoDevice != value)
                {
                    videoDevice = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Video codec.
        /// </summary>
        public FFmpegVideoCodec VideoCodec
        {
            get { return videoCodec; }

            set
            {
                if (videoCodec != value)
                {
                    videoCodec = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Audio codec.
        /// </summary>
        public FFmpegAudioCodec AudioCodec
        {
            get { return audioCodec; }

            set
            {
                if (audioCodec != value)
                {
                    audioCodec = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// User's custom arguments.
        /// </summary>
        public string UserArgs
        {
            get { return userArgs; }

            set
            {
                if (userArgs != value)
                {
                    userArgs = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// X264 preset.
        /// </summary>
        public FFmpegX264Preset X264Preset
        {
            get { return x264Preset; }

            set
            {
                if (x264Preset != value)
                {
                    x264Preset = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// VP9 CRF.
        /// </summary>
        public int Vp9Crf
        {
            get { return vp9Crf; }

            set
            {
                if (vp9Crf != value)
                {
                    if (value >= 0 && value <= 63)
                    {
                        vp9Crf = value;
                        OnPropertyChanged();
                    }
                }
            }
        }

        /// <summary>
        /// X264 CRF.
        /// </summary>
        public int X264Crf
        {
            get { return x264Crf; }

            set
            {
                if (x264Crf != value)
                {
                    if (value >= 0 && value <= 51)
                    {
                        x264Crf = value;
                        OnPropertyChanged();
                    }
                }
            }
        }

        /// <summary>
        /// XviD quality scale.
        /// </summary>
        public int XviDQscale
        {
            get { return xviDQscale; }

            set
            {
                if (xviDQscale != value)
                {
                    if (value >= 1 && value <= 31)
                    {
                        xviDQscale = value;
                        OnPropertyChanged();
                    }
                }
            }
        }

        /// <summary>
        /// VP9 bit rate.
        /// </summary>
        public int Vp9Bitrate
        {
            get { return vp9Bitrate; }

            set
            {
                if (vp9Bitrate != value && value > 0)
                {
                    vp9Bitrate = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// AAC quality scale.
        /// </summary>
        public int AacQScale
        {
            get { return aacQScale; }

            set
            {
                if (aacQScale != value)
                {
                    if (value >= 1 && value <= 5)
                    {
                        aacQScale = value;
                        OnPropertyChanged();
                    }
                }
            }
        }

        /// <summary>
        /// Opus bit rate.
        /// </summary>
        public int OpusBitrate
        {
            get { return opusBitrate; }

            set
            {
                if (opusBitrate != value)
                {
                    if (value > 0)
                    {
                        opusBitrate = value;
                        OnPropertyChanged();
                    }
                }
            }
        }

        /// <summary>
        /// Vorbis quality scale.
        /// </summary>
        public int VorbisQscale
        {
            get { return vorbisQscale; }

            set
            {
                if (vorbisQscale != value)
                {
                    if (value >= 0 && value <= 10)
                    {
                        vorbisQscale = value;
                        OnPropertyChanged();
                    }
                }
            }
        }

        /// <summary>
        /// MP3 quality scale.
        /// </summary>
        public int MP3Qscale
        {
            get { return mp3Qscale; }

            set
            {
                if (mp3Qscale != value)
                {
                    if (value >= 0 && value <= 9)
                    {
                        mp3Qscale = value;
                        OnPropertyChanged();
                    }
                }
            }
        }

        /// <summary>
        /// Video extension.
        /// </summary>
        public string Extension
        {
            get
            {
                switch (VideoCodec)
                {
                    case FFmpegVideoCodec.Libx264:
                    case FFmpegVideoCodec.Libx265:
                        return "mp4";
                    case FFmpegVideoCodec.Libvpx_vp9:
                        return "webm";
                    case FFmpegVideoCodec.Libxvid:
                        return "avi";
                }

                return "mp4";
            }
        }

        /// <inheritdoc/>
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string memberName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
        }

        /// <summary>
        /// Clone.
        /// </summary>
        public FFmpegOptions Clone()
        {
            return MemberwiseClone() as FFmpegOptions;
        }

        object ICloneable.Clone()
        {
            return Clone();
        }
    }
}
