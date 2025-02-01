using UoW_API.Repositories.Data;
using UoW_API.Repositories.Entities.Dtos.Project;
using UoW_API.Repositories.Repository.Interfaces;

namespace UoW_API.Repositories.Repository;
public class ProjectRepository : IProjectRepository
{
    private readonly DataContext _context;

    public ProjectRepository(DataContext context)
    {
        _context = context;
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
