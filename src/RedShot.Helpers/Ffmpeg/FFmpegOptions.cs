using RedShot.Helpers.Ffmpeg.Devices;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace RedShot.Helpers.Ffmpeg
{
    public class FFmpegOptions : INotifyPropertyChanged
    {
        private int fps;
        private bool drawCursor;
        private Device videoDevice;
        private Device audioDevice;
        private FFmpegVideoCodec videoCodec;
        private FFmpegAudioCodec audioCodec;
        private FFmpegX264Preset x264Preset;
        private int x264Crf;
        private int xviDQscale;
        private int bitrate;
        private FFmpegNVENCPreset nvencPreset;
        private FFmpegQSVPreset qsvPreset;
        private int aacBitrate;
        private int opusBitrate;
        private int vorbisQscale;
        private int mp3Qscale;
        private string userArgs;
        private bool useGdigrab;

        public FFmpegOptions()
        {
            Fps = 30;
            DrawCursor = true;
            VideoCodec = FFmpegVideoCodec.libx264;
            AudioCodec = FFmpegAudioCodec.libvoaacenc;
            X264Preset = FFmpegX264Preset.faster;
            X264Crf = 28;
            XviDQscale = 10;
            Bitrate = 3000;
            NVENCPreset = FFmpegNVENCPreset.llhp;
            QSVPreset = FFmpegQSVPreset.fast;
            AACBitrate = 128;
            OpusBitrate = 128;
            VorbisQscale = 3;
            MP3Qscale = 4;
            UserArgs = string.Empty;
            UseGdigrab = true;
        }

        public int Fps
        {
            get { return fps; }

            set
            {
                if (fps != value)
                {
                    fps = value;
                    OnPropertyChanged();
                }
            }
        }

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

        // Windows only.
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

        public Device AudioDevice
        {
            get { return audioDevice; }

            set
            {
                if (audioDevice != value)
                {
                    audioDevice = value;
                    OnPropertyChanged();
                }
            }
        }

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

        // Video
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

        public int X264Crf
        {
            get { return x264Crf; }

            set
            {
                if (x264Crf != value)
                {
                    x264Crf = value;
                    OnPropertyChanged();
                }
            }
        }

        public int XviDQscale
        {
            get { return xviDQscale; }

            set
            {
                if (xviDQscale != value)
                {
                    xviDQscale = value;
                    OnPropertyChanged();
                }
            }
        }


        public int Bitrate
        {
            get { return bitrate; }

            set
            {
                if (bitrate != value)
                {
                    bitrate = value;
                    OnPropertyChanged();
                }
            }
        }

        public FFmpegNVENCPreset NVENCPreset
        {
            get { return nvencPreset; }

            set
            {
                if (nvencPreset != value)
                {
                    nvencPreset = value;
                    OnPropertyChanged();
                }
            }
        }

        public FFmpegQSVPreset QSVPreset
        {
            get { return qsvPreset; }

            set
            {
                if (qsvPreset != value)
                {
                    qsvPreset = value;
                    OnPropertyChanged();
                }
            }
        }

        // Audio
        public int AACBitrate
        {
            get { return aacBitrate; }

            set
            {
                if (aacBitrate != value)
                {
                    aacBitrate = value;
                    OnPropertyChanged();
                }
            }
        }

        public int OpusBitrate
        {
            get { return opusBitrate; }

            set
            {
                if (opusBitrate != value)
                {
                    opusBitrate = value;
                    OnPropertyChanged();
                }
            }
        }

        public int VorbisQscale
        {
            get { return vorbisQscale; }

            set
            {
                if (vorbisQscale != value)
                {
                    vorbisQscale = value;
                    OnPropertyChanged();
                }
            }
        }

        public int MP3Qscale
        {
            get { return mp3Qscale; }

            set
            {
                if (mp3Qscale != value)
                {
                    mp3Qscale = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Extension
        {
            get
            {
                switch (VideoCodec)
                {
                    case FFmpegVideoCodec.libx264:
                    case FFmpegVideoCodec.h264_qsv:
                    case FFmpegVideoCodec.h264_nvenc:
                        return "mp4";
                    case FFmpegVideoCodec.libvpx_vp9:
                        return "webm";
                    case FFmpegVideoCodec.libxvid:
                        return "avi";
                }

                return "mp4";
            }
        }

        void OnPropertyChanged([CallerMemberName] string memberName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
