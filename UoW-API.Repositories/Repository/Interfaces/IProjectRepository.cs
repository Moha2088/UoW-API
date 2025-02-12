using UoW_API.Repositories.Entities;
using UoW_API.Repositories.Entities.Dtos.Project;

namespace UoW_API.Repositories.Repository.Interfaces;
public interface IProjectRepository : IGenericRepository<Project>
{
    Task AddUser(int id, int projectId, CancellationToken cancellationToken);
    Task GeneratePDF(int projectId, CancellationToken cancellationToken);
}
