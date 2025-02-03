using AutoMapper;
using Microsoft.EntityFrameworkCore;
using UoW_API.Repositories.Data;
using UoW_API.Repositories.Entities;
using UoW_API.Repositories.Entities.Dtos.Project;
using UoW_API.Repositories.Repository.Interfaces;

namespace UoW_API.Repositories.Repository;
public class ProjectRepository : IProjectRepository
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public ProjectRepository(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ProjectGetDto> CreateProject(ProjectCreateDto dto)
    {
        var dbProject = _mapper.Map<Project>(dto);
        _context.Projects.Add(dbProject);
        return _mapper.Map<ProjectGetDto>(dbProject);
    }


    public async Task DeleteProject(int id)
    {
        var dbProject = await _context.Projects.FindAsync(id);
        _context.Remove(dbProject);
    }

    public async Task<IEnumerable<ProjectGetDto>> GetProjects()
    {
        var dbProjects = await _context.Projects.ToListAsync(); ;
        return _mapper.Map<List<ProjectGetDto>>(dbProjects);
    }

    public async Task<ProjectGetDto> GetProject(int id)
    {
        var dbProject = await _context.Projects.FindAsync(id);
        return _mapper.Map<ProjectGetDto>(dbProject);
    }
}
