using AutoMapper;
using Microsoft.EntityFrameworkCore;
using UoW_API.Repositories.Data;
using UoW_API.Repositories.Entities;
using UoW_API.Repositories.Entities.Dtos.Project;
using UoW_API.Repositories.Entities.Dtos.User;
using UoW_API.Repositories.Enums;
using UoW_API.Repositories.Repository.Interfaces;

namespace UoW_API.Repositories.Repository;
public class ProjectRepository : GenericRepository<Project>, IProjectRepository
{

    public ProjectRepository(DataContext context): base(context)
    {

    }


    public override async Task<Project> Get(int id, CancellationToken cancellationToken)
    {
        var dbProject =  await _context.Projects.FindAsync(id, cancellationToken);
        return dbProject!;
    }

    public override async Task<IEnumerable<Project>> GetAll(CancellationToken cancellationToken)
    {
        var dbProjects = await _context.Projects
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return dbProjects;
    }

    public override void Create(Project dbProject, CancellationToken cancellationToken)
    {


        if (dbProject.From < DateTimeOffset.Now && DateTimeOffset.Now < dbProject.To)
        {
            dbProject.State = CurrentState.ONGOING;
        }

        if (dbProject.From > DateTimeOffset.Now)
        {
            dbProject.State = CurrentState.PENDING;
        }

        else
        {
            dbProject.State = CurrentState.FINISHED;
        }

        _context.Projects.Add(dbProject);
    }

    public override async Task Delete(int id, CancellationToken cancellationToken)
    {
        var dbProject = await _context.Projects.FindAsync(id, cancellationToken);
        _context.Projects.Remove(dbProject!);
    }
}
