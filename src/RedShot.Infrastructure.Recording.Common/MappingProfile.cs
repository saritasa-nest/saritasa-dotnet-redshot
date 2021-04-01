using AutoMapper;
using RedShot.Infrastructure.Configuration.Models.Recording;
using RedShot.Infrastructure.Recording.Common.Devices;
using RedShot.Infrastructure.Recording.Common.Ffmpeg;

namespace RedShot.Infrastructure.Recording.Common
{
    /// <summary>
    /// Mapping profile for recording configuration.
    /// </summary>
    public class MappingProfile : Profile
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public MappingProfile()
        {
            CreateMap<DeviceData, Device>().ReverseMap();
            CreateMap<AudioData, AudioOptions>().ReverseMap();
            CreateMap<FFmpegData, FFmpegOptions>().ReverseMap();

            CreateMap<RecordingConfiguration, RecordingOptions>()
                        .ForMember(c => c.AudioOptions, opt => opt.MapFrom(o => o.AudioData))
                        .ForMember(c => c.FFmpegOptions, opt => opt.MapFrom(o => o.FFmpegData))
                        .ReverseMap();
        }
    }
}
