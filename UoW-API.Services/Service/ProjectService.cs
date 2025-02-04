using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UoW_API.Repositories.Entities.Dtos.Project;
using UoW_API.Repositories.UnitOfWork.Interfaces;
using UoW_API.Services.Interfaces;

namespace UoW_API.Services.Service;
public class ProjectService : IProjectService
{

    private readonly IUnitOfWork _unitOfWork;

    public ProjectService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
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
        try
        {
            return await _unitOfWork.ProjectRepository.GetProject(id, cancellationToken);
        }

        catch (InvalidOperationException) 
        {
            throw;
        } 
    }

    public async Task<IEnumerable<ProjectGetDto>> GetProjects(CancellationToken cancellationToken)
    {
        return await _unitOfWork.ProjectRepository.GetProjects(cancellationToken);
    }
}
