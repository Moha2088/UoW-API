using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UoW_API.Repositories.Data;
using UoW_API.Repositories.Repository.Interfaces;

namespace UoW_API.Repositories.Repository;
public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    internal DataContext _context;
    protected DbSet<T> _dbSet;

    public GenericRepository(DataContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }


    public async virtual Task<T> Get(int id, CancellationToken cancellationToken)
    {
        return await _dbSet.FindAsync(id)!; 
    }

    public async virtual Task<IEnumerable<T>> GetAll(CancellationToken cancellationToken)
    {
        return await _dbSet.ToListAsync();
    }

    public virtual void Create(T entity, CancellationToken cancellationToken)
    {
        _dbSet.Add(entity);
    }

    public async virtual Task Delete(int id, CancellationToken cancellationToken)
    {
        T entity = await _dbSet.FindAsync(id)!;

        if (_context.Entry(entity).State == EntityState.Detached)
        {
            _dbSet.Attach(entity);
        }

        _dbSet.Remove(entity);
    }
}
