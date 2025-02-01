using UoW_API.Repositories.Repository.Interfaces;

namespace UoW_API.Repositories.UnitOfWork.Interfaces;

public interface IUnitOfWork
{
    Task<bool> SaveChangesAsync(CancellationToken cancellationToken);
    IProjectRepository ProjectRepository { get; }
    IUserRepository UserRepository { get; }
}