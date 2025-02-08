using UoW_API.Repositories.Entities.Dtos.User;
using UoW_API.Repositories.UnitOfWork.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UoW_API.Services.Interfaces;
using UoW_API.Repositories.Entities;

namespace UoW_API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ILogger<UsersController> _logger;

    public UsersController(IUserService userService, ILogger<UsersController> logger) =>
        (_userService, _logger) = (userService, logger);



    /// <summary>
    /// Creates a user
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///     
    ///     POST /users
    ///     {
    ///         "name": "User"
    ///     }
    /// 
    /// </remarks>
    /// <param name="dto">Create user dto</param>
    /// <param name="cancellationToken">A cancellation token</param>
    /// <response code="200">Returns OK with the created user</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(UserGetDto))]
    public async Task<IActionResult> CreateUser([FromBody] User y, CancellationToken cancellationToken)
    {
        await _userService.CreateUser(y, cancellationToken);
        return Created();
    }

    /// <summary>
    /// Gets a user
    /// </summary>
    /// <param name="id">Id of the user</param>
    /// <param name="cancellationToken">A cancellation token</param>
    /// <response code= "200">Returns ok if the user exists</response>
    /// <response code= "200">Returns Not Found if the user doesn't exist</response>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserGetDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUser([FromRoute] int id, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _userService.GetUser(id, cancellationToken);
            return Ok(result);
        }

        catch (InvalidOperationException e)
        {
            return NotFound();
        }
    }

    /// <summary>
    /// Gets a list of users
    /// </summary>
    /// <param name="cancellationToken">A cancellation token</param>
    /// <response code ="200">Returns ok with a list of users</response>
    /// <response code ="404">Returns Not Found if no users exists</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserGetDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUsers(CancellationToken cancellationToken)
    {
        var results = await _userService.GetUsers(cancellationToken);
        return results.Any() ? Ok(results) : NotFound();
    }

    /// <summary>
    /// Uploads an image to the cloud
    /// </summary>
    /// <param name="id">Id of the user</param>
    /// <param name="localFilePath">The path of the image to upload</param>
    /// <param name="cancellationToken">A cancellation token</param>
    /// <response code="200">Returns OK if the user exists</response>
    /// <response code="404">Returns NotFound if the user doesn't exist</response>
    [HttpPost("upload/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UploadImage([FromRoute] int id, string localFilePath, CancellationToken cancellationToken)
    {
        try
        {
            await _userService.UploadImageAsync(id, localFilePath, cancellationToken);
            return Ok("Successful upload!");
        }

        catch (InvalidOperationException e)
        {
            return NotFound();
        }
    }

    /// <summary>
    /// Deletes a user
    /// </summary>
    /// <param name="id">Id of the user</param>
    /// <param name="cancellationToken">A cancellation token</param>
    /// <response code="204">Returns NoContent</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent, Type = typeof(UserGetDto))]
    public async Task<IActionResult> DeleteUser([FromRoute] int id, CancellationToken cancellationToken)
    {
        await _userService.DeleteUser(id, cancellationToken);
        return NoContent();
    }
}