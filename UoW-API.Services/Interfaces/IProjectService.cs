﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UoW_API.Repositories.Entities.Dtos.Project;

namespace UoW_API.Services.Interfaces;
public interface IProjectService
{
    Task<ProjectGetDto> CreateProject(ProjectCreateDto dto, CancellationToken cancellationToken);
    Task<IEnumerable<ProjectGetDto>> GetProjects(CancellationToken cancellationToken);
    Task<ProjectGetDto> GetProject(int id, CancellationToken cancellationToken);
    Task DeleteProject(int id, CancellationToken cancellationToken);
}
