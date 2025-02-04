using UoW_API.Repositories.Entities.Dtos.Project;

namespace UoW_API.Repositories.Repository.Interfaces;
public interface IProjectRepository
{
    Task<ProjectGetDto> CreateProject(ProjectCreateDto dto);
    Task<IEnumerable<ProjectGetDto>> GetProjects(CancellationToken cancellationToken);
    Task<ProjectGetDto> GetProject(int id, CancellationToken cancellationToken);
    Task DeleteProject(int id, CancellationToken cancellationToken);
}
