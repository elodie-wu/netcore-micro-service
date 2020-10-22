using AutoMapper;
using MicroService.Basic.Domain.Entity;
using MicroService.Basic.DTO.VM;
using System;
using System.Collections.Generic;
using System.Text;

namespace MicroService.Basic.DTO.AutoMapper
{
    public class AutoMapperConfig : Profile
    { 
        public AutoMapperConfig()
        {
            CreateMap<UserEntity, UserInfo>();
        }
    }
}
