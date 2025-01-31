using Clean_Project_Backend.Repositories.Data;
using Clean_Project_Backend.Repositories.Entities;
using Clean_Project_Backend.Repositories.Repository.Interfaces;
using Clean_Project_Backend.Repositories.UnitOfWork.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clean_Project_Backend.Repositories.UnitOfWork;

public class UnitOfWork : IUnitOfWork, IDisposable
{
    private readonly DataContext _context;
    public IUserRepository UserRepository { get; }
    public IProjectRepository ProjectRepository { get; }
    private ILogger<UnitOfWork> _logger;

    public UnitOfWork(DataContext context, IUserRepository userRepository, IProjectRepository projectRepository, ILogger<UnitOfWork> logger)
    {
        _context = context;
        UserRepository = userRepository;
        ProjectRepository = projectRepository;
        _logger = logger;
    }


    public void Dispose()
    {
        _context.Dispose();
    }

    public async Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        bool succeeded = true;

        using var transaction = _context.Database.BeginTransaction();

        try
        {
            await _context.SaveChangesAsync(cancellationToken);
            transaction.Commit();
            return succeeded;
        }

        catch (OperationCanceledException)
        {
            _logger.LogError("Changes could not be saved! Rolling back changes.");
            transaction.Rollback();
            succeeded = false;
        }

        return succeeded;
    }
}
