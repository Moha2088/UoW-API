using Clean_Project_Backend.Repositories.Repository.Interfaces;

namespace Clean_Project_Backend.Repositories.UnitOfWork.Interfaces;

public interface IUnitOfWork
{
    Task<bool> SaveChangesAsync(CancellationToken cancellationToken);
    IProjectRepository ProjectRepository { get; }
    IUserRepository UserRepository { get; }
}