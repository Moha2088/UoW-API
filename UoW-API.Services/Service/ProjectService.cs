using AutoMapper;
using Azure;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UoW_API.Repositories.Entities;
using UoW_API.Repositories.Entities.Dtos.Project;
using UoW_API.Repositories.Entities.Dtos.User;
using UoW_API.Repositories.Exceptions;
using UoW_API.Repositories.Repository.Caching.Interfaces;
using UoW_API.Repositories.UnitOfWork.Interfaces;
using UoW_API.Services.Interfaces;

namespace UoW_API.Services.Service;
public class ProjectService : IProjectService
{

    private readonly IUnitOfWork _unitOfWork;
    private readonly IRedisCacheService _cacheService;
    private readonly ILogger<ProjectService> _logger;
    private readonly IMapper _mapper;
    private const string _getProjectsCachingKey = "GET_PROJECTS";


    public ProjectService(IUnitOfWork unitOfWork, IRedisCacheService cacheService, ILogger<ProjectService> logger, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _cacheService = cacheService;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task CreateProject(ProjectCreateDto dto, CancellationToken cancellationToken)
    {
        var dbProject = _mapper.Map<Project>(dto);
        _unitOfWork.ProjectRepository.Create(dbProject, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteProject(int id, CancellationToken cancellationToken)
    {
        try
        {
            await _unitOfWork.ProjectRepository.Delete(id, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        catch (ProjectNotFoundException) 
        {
            throw;
        }
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
            var dbProject = await _unitOfWork.ProjectRepository.Get(id, cancellationToken);
            var dto = _mapper.Map<ProjectGetDto>(dbProject);

            if (dto is not null)
            {
                _cacheService.Set(id.ToString(), dto);
                return dto;
            }
        }

        catch (ProjectNotFoundException)
        {
            throw;
        }

        return null!;
    }

    public async Task<IEnumerable<ProjectGetDto>> GetProjects(CancellationToken cancellationToken)
    {
        var cachedProjects = _cacheService.Get<List<ProjectGetDto>>(_getProjectsCachingKey);

        if (cachedProjects is not null)
        {
            _logger.LogInformation("Retrieving projects from cache");
            return cachedProjects;
        }

        var dbProjects = await _unitOfWork.ProjectRepository.GetAll(cancellationToken);
        var dto = _mapper.Map<List<ProjectGetDto>>(dbProjects);

        if (dbProjects.Any())
        {
            _cacheService.Set(_getProjectsCachingKey, dto);
        }

        return dto;
    }

    public async Task AddUser(int id, int projectId, CancellationToken cancellationToken)
    {
        try
        {
            await _unitOfWork.ProjectRepository.AddUser(id, projectId, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        catch (ProjectNotFoundException) 
        {
            throw;
        }

        catch (UserNotFoundException)
        {
            throw;
        }
        
    }

    public async Task GeneratePDF(int projectId, CancellationToken cancellationToken)
    {
        try
        {
            await _unitOfWork.ProjectRepository.GeneratePDF(projectId, cancellationToken);
        }

        catch (RequestFailedException)
        {
            throw;
        }
    }
}
