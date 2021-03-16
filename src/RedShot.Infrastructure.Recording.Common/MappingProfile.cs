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
            CreateMap<FFmpegData, FFmpegOptions>()
                        .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != default));
            CreateMap<FFmpegOptions, FFmpegData>();

            CreateMap<RecordingConfiguration, RecordingOptions>()
                        .ForMember(c => c.AudioOptions, opt => opt.MapFrom(o => o.AudioData))
                        .ForMember(c => c.FFmpegOptions, opt => opt.MapFrom(o => o.FFmpegData))
                        .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != default));

            CreateMap<RecordingOptions, RecordingConfiguration>()
                        .ForMember(c => c.AudioData, opt => opt.MapFrom(o => o.AudioOptions))
                        .ForMember(c => c.FFmpegData, opt => opt.MapFrom(o => o.FFmpegOptions));
        }
    }
}
