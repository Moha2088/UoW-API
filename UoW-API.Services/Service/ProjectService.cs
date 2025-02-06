using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UoW_API.Repositories.Entities.Dtos.Project;
using UoW_API.Repositories.Entities.Dtos.User;
using UoW_API.Repositories.Repository.Caching.Interfaces;
using UoW_API.Repositories.UnitOfWork.Interfaces;
using UoW_API.Services.Interfaces;

namespace UoW_API.Services.Service;
public class ProjectService : IProjectService
{

    private readonly IUnitOfWork _unitOfWork;
    private readonly IRedisCacheService _cacheService;
    private readonly ILogger<ProjectService> _logger;
    private const string _getProjectsCachingKey = "GET_PROJECTS";


    public ProjectService(IUnitOfWork unitOfWork, IRedisCacheService cacheService, ILogger<ProjectService> logger)
    {
        _unitOfWork = unitOfWork;
        _cacheService = cacheService;
        _logger = logger;
    }

    public async Task<ProjectGetDto> CreateProject(ProjectCreateDto dto, CancellationToken cancellationToken)
    {
        var dbProject = await _unitOfWork.ProjectRepository.CreateProject(dto);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return dbProject;
    }

    public async Task DeleteProject(int id, CancellationToken cancellationToken)
    {
        await _unitOfWork.ProjectRepository.DeleteProject(id, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task<ProjectGetDto> GetProject(int id, CancellationToken cancellationToken)
    {
        var cachedProject = _cacheService.Get<ProjectGetDto>(id.ToString());

        if (cachedProject is not null)
        {
            _logger.LogInformation("Retrieving project from cache");
            return cachedProject;
        }

        try
        {
            var dbProject = await _unitOfWork.ProjectRepository.GetProject(id, cancellationToken);

            if (dbProject is not null)
            {
                _cacheService.Set(id.ToString(), dbProject);
            }
        }

        catch (InvalidOperationException)
        {
            throw;
        }

        return (ProjectGetDto)Enumerable.Empty<ProjectGetDto>();
    }

    public async Task<IEnumerable<ProjectGetDto>> GetProjects(CancellationToken cancellationToken)
    {
        var cachedProjects = _cacheService.Get<List<ProjectGetDto>>(_getProjectsCachingKey);

        if (cachedProjects is not null)
        {
            _logger.LogInformation("Retrieving projects from cache");
            return cachedProjects;
        }

        var dbProjects = await _unitOfWork.ProjectRepository.GetProjects(cancellationToken);

        if (dbProjects.Any())
        {
            _cacheService.Set(_getProjectsCachingKey, dbProjects);
        }

        return dbProjects;
    }
}
