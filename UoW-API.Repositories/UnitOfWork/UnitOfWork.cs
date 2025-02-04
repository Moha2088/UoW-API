﻿using UoW_API.Repositories.Data;
using UoW_API.Repositories.Repository.Interfaces;
using UoW_API.Repositories.UnitOfWork.Interfaces;
using Microsoft.Extensions.Logging;

namespace UoW_API.Repositories.UnitOfWork;

public class UnitOfWork : IUnitOfWork, IDisposable
{
    private readonly DataContext _context;
    public IUserRepository UserRepository { get; }
    public IProjectRepository ProjectRepository { get; }
    private ILogger<UnitOfWork> _logger;
    private bool disposed = false;

    public UnitOfWork(DataContext context, IUserRepository userRepository, IProjectRepository projectRepository, ILogger<UnitOfWork> logger)
    {
        _context = context;
        UserRepository = userRepository;
        ProjectRepository = projectRepository;
        _logger = logger;
    }


    protected virtual void Dispose(bool disposing)
    {
        if (!disposed)
        {
            if (disposing)
            {
                _context.Dispose();
            }
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
      await _context.SaveChangesAsync(cancellationToken);
    }
}
