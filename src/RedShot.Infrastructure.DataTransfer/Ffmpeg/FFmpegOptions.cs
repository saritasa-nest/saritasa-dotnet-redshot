﻿using RedShot.Infrastructure.DataTransfer.Ffmpeg.Devices;
using RedShot.Infrastructure.DataTransfer.Ffmpeg.Encoding;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace RedShot.Infrastructure.DataTransfer.Ffmpeg
{
    public class FFmpegOptions : INotifyPropertyChanged, ICloneable
    {
        private int fps;
        private bool drawCursor;
        private Device videoDevice;
        private Device audioDevice;
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
        private bool useMicrophone;

        public FFmpegOptions()
        {
            Fps = 30;
            DrawCursor = true;
            VideoCodec = FFmpegVideoCodec.libx264;
            AudioCodec = FFmpegAudioCodec.libvoaacenc;
            X264Preset = FFmpegX264Preset.faster;
            X264Crf = 23;
            Vp9Crf = 35;
            XviDQscale = 10;
            Vp9Bitrate = 3000;
            AacQScale = 3;
            OpusBitrate = 128;
            VorbisQscale = 3;
            MP3Qscale = 4;
            UserArgs = string.Empty;
            UseGdigrab = true;
            useMicrophone = true;
        }

        public bool UseMicrophone
        {
            get { return useMicrophone; }

            set
            {
                if (useMicrophone != value)
                {
                    useMicrophone = value;
                    OnPropertyChanged();
                }
            }
        }

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

        // Audio
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

        public string Extension
        {
            get
            {
                switch (VideoCodec)
                {
                    case FFmpegVideoCodec.libx264:
                    case FFmpegVideoCodec.libx265:
                        return "mp4";
                    case FFmpegVideoCodec.libvpx_vp9:
                        return "webm";
                    case FFmpegVideoCodec.libxvid:
                        return "avi";
                }

                return "mp4";
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string memberName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
        }

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
