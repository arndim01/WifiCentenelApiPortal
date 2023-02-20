using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WifiCentenelApiPortal.Models.Mappings
{
    public class ViewModelToEntityMappingProfile : Profile
    {
        public ViewModelToEntityMappingProfile()
        {
            CreateMap<RegistrationDetails, ApplicationUser>().ForMember(au => au.UserName, map => map.MapFrom(vm => vm.UserName));
        }
    }
}
