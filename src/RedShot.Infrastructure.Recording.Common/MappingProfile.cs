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

            CreateMap<RecordingConfiguration, FFmpegConfigurationOption>()
                     .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != default));

            CreateMap<FFmpegConfigurationOption, RecordingConfiguration>();
        }
    }
}
