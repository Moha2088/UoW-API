using Clean_Project_Backend.Repositories.Entities.Dtos.Project;
using Clean_Project_Backend.Repositories.Entities.Dtos.User;

namespace Clean_Project_Backend.Repositories.Repository.Interfaces;
public interface IProjectRepository
{
    Task<ProjectGetDto> CreateProject(ProjectCreateDto dto);
    Task<IEnumerable<ProjectGetDto>> GetProject();
    Task<ProjectGetDto> GetProject(int id);
    Task DeleteProject(int id);
}
