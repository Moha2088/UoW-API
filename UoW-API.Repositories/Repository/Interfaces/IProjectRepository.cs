using UoW_API.Repositories.Entities.Dtos.Project;

namespace UoW_API.Repositories.Repository.Interfaces;
public interface IProjectRepository
{
    Task<ProjectGetDto> CreateProject(ProjectCreateDto dto);
    Task<IEnumerable<ProjectGetDto>> GetProject();
    Task<ProjectGetDto> GetProject(int id);
    Task DeleteProject(int id);
}
