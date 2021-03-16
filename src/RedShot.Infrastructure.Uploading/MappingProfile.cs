using AutoMapper;
using RedShot.Infrastructure.Uploading.Uploaders.Ftp;
using RedShot.Infrastructure.Uploading.Uploaders.Ftp.Models;
using RedShot.Infrastructure.Configuration.Models.Account;

namespace RedShot.Infrastructure.Uploading
{
    /// <summary>
    /// An automapper mapping profile for all entities related to uploading module.
    /// </summary>
    public class MappingProfile : Profile
    {
        /// <summary>
        /// Constructor. Registers new mappings.
        /// </summary>
        public MappingProfile()
        {
            CreateMap<AccountConfiguration, FtpOptions>()
                .ForMember(target => target.FtpAccounts, config => config.MapFrom(source => source.Accounts))
                .ReverseMap();
            CreateMap<AccountData, FtpAccount>()
                .ReverseMap();
        }
    }
}
