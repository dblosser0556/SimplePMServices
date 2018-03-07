using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

using SimplePMServices.Models.Entities;

namespace SimplePMServices.ViewModels.Mappings
{
    public class ViewModelToEntityMappingProfile : Profile
    {
        public ViewModelToEntityMappingProfile()
        {
            CreateMap<LoggedInUser, AppUser>().ForMember(au => au.UserName, map => map.MapFrom(vm => vm.CurrentUser.UserName));
        }
    }
}
