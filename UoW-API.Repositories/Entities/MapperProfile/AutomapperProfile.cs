using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using System.Threading.Tasks;
using UoW_API.Repositories.Entities.Dtos.User;
using UoW_API.Repositories.Entities.Dtos.Project;

namespace UoW_API.Repositories.Entities.MapperProfile;
public class AutomapperProfile : Profile
{
    public AutomapperProfile()
    {
        #region User
        CreateMap<UserCreateDto, User>();
        CreateMap<User, UserGetDto>();
        #endregion


        #region Project
        CreateMap<ProjectCreateDto, Project>();
        CreateMap<Project, ProjectGetDto>();
        #endregion

        var automapperConfig = new MapperConfiguration(x => x.AddProfile<AutomapperProfile>());
        automapperConfig.AssertConfigurationIsValid();
    }
}
