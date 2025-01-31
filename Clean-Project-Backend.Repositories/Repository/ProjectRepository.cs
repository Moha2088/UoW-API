using Clean_Project_Backend.Repositories.Entities.Dtos.Project;
using Clean_Project_Backend.Repositories.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clean_Project_Backend.Repositories.Repository;
public class ProjectRepository : IProjectRepository
{
    public ProjectRepository()
    {
        
    }

    public Task<ProjectGetDto> CreateProject(ProjectCreateDto dto)
    {
        throw new NotImplementedException();
    }

    public Task DeleteProject(int id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<ProjectGetDto>> GetProject()
    {
        throw new NotImplementedException();
    }

    public Task<ProjectGetDto> GetProject(int id)
    {
        throw new NotImplementedException();
    }
}
