using UoW_API.Repositories.Entities;
using UoW_API.Repositories.Repository;
using UoW_API.Repositories.Repository.Interfaces;

namespace UoW_API.Repositories.UnitOfWork.Interfaces;

public interface IUnitOfWork
{
    Task SaveChangesAsync(CancellationToken cancellationToken);
    IUserRepository UserRepository { get; }
    IProjectRepository ProjectRepository { get; }
}