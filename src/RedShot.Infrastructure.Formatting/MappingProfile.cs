﻿using AutoMapper;
using RedShot.Infrastructure.Configuration.Models.General;

namespace RedShot.Infrastructure.Formatting
{
    /// <summary>
    /// Mapping profile for general configuration.
    /// </summary>
    public class MappingProfile : Profile
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public MappingProfile()
        {
            CreateMap<GeneralConfiguration, GeneralOptions>()
                     .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != default));

            CreateMap<GeneralOptions, GeneralConfiguration>();
        }
    }
}
