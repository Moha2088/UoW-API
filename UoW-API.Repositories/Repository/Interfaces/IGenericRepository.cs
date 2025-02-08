namespace UoW_API.Repositories.Repository.Interfaces;

public interface IGenericRepository<T> where T : class
{
    void Create(T entity, CancellationToken cancellationToken);
    Task Delete(int id, CancellationToken cancellationToken);
    Task<IEnumerable<T>> GetAll(CancellationToken cancellationToken);
    Task<T> Get(int id, CancellationToken cancellationToken);
}