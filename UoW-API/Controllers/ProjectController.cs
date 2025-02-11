using Microsoft.AspNetCore.Mvc;
using UoW_API.Repositories.Entities;
using UoW_API.Repositories.Entities.Dtos.Project;
using UoW_API.Repositories.Entities.Dtos.User;
using UoW_API.Repositories.Exceptions;
using UoW_API.Repositories.UnitOfWork.Interfaces;
using UoW_API.Services.Interfaces;

namespace UoW_API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProjectController : ControllerBase
{
    private readonly IProjectService _projectService;

    public ProjectController(IProjectService projectService)
    {
        _projectService = projectService;
    }

    /// <summary>
    /// Creates a project
    /// </summary>
    /// <param name="dto">Create project dto</param>
    /// <param name="cancellationToken">A cancellation token</param>
    /// <response code="201">Returns created with the project</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ProjectGetDto))]
    public async Task<IActionResult> CreateProject([FromBody] ProjectCreateDto dto, CancellationToken cancellationToken)
    {
        await _projectService.CreateProject(dto, cancellationToken);
        return Created();
    }

    /// <summary>
    /// Gets a project
    /// </summary>
    /// <param name="id">Id of the project</param>
    /// <param name="cancellationToken">A cancellation token</param>
    /// <response code= "200">Returns OK with the project</response>
    /// <response code= "404">Returns NotFound if the project doesn't exist</response>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProjectGetDto))]
    public async Task<IActionResult> GetProject([FromRoute] int id, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _projectService.GetProject(id, cancellationToken);
            return Ok(result);
        }

        catch (ProjectNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    /// <summary>
    /// Gets a list of projects
    /// </summary>
    /// <param name="cancellationToken">A cancellation token</param>
    /// <response code= "200">Returns OK with the projects</response>
    /// <response code= "200">Returns NotFound if no projects exists</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProjects(CancellationToken cancellationToken)
    {
        var results = await _projectService.GetProjects(cancellationToken);
        return results.Any() ? Ok(results) : NotFound();
    }

    /// <summary>
    /// Deletes a project
    /// </summary>
    /// <param name="id">Id of the project</param>
    /// <param name="cancellationToken">A cancellation token</param>
    /// <response code = "204">Returns NoContent</response>
    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent, Type = typeof(UserGetDto))]
    public async Task<IActionResult> DeleteProject([FromRoute] int id, CancellationToken cancellationToken)
    {
        await _projectService.DeleteProject(id, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Adds a user to a project
    /// </summary>
    /// <param name="id">Id of the user</param>
    /// <param name="projectId">Id of the project</param>
    /// <param name="cancellationToken">A cancellation token</param>
    /// <response code = "200">Returns OK</response>
    /// <response code = "404">Returns NotFound if the project or user doesn't exist</response>
    [HttpPost("addUser/{id}/{projectId}")]
    public async Task<IActionResult> AddUser([FromRoute] int id, int projectId, CancellationToken cancellationToken)
    {
        try
        {
            await _projectService.AddUser(id, projectId, cancellationToken);
            return NoContent();
        }

        catch (ProjectNotFoundException e)
        {
            return NotFound(e.Message);
        }

        catch (UserNotFoundException e) 
        {
            return NotFound(e.Message);
        }
    }
}
