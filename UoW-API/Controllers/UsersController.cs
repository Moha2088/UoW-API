using UoW_API.Repositories.Entities.Dtos.User;
using UoW_API.Repositories.UnitOfWork.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UoW_API.Services.Interfaces;

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
    /// <remarks>
    /// <param name="dto">Create user dto</param>
    /// <param name="cancellationToken">A cancellation token</param>
    /// <response code="200">Returns OK with the created user</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(UserGetDto))]
    public async Task<IActionResult> CreateUser([FromBody] UserCreateDto dto, CancellationToken cancellationToken)
    {
        var result = await _userService.CreateUser(dto, cancellationToken);
        return Created(nameof(CreateUser), result);
    }

    ///// <summary>
    ///// Gets a user
    ///// </summary>
    ///// <param name="id">Id of the user</param>
    ///// <param name="cancellationToken">A cancellation token</param>
    ///// <response code= "200">Returns ok if the user exists</response>
    ///// <response code= "200">Returns Not Found if the user doesn't exist</response>
    //[HttpGet("{id}")]
    //[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserGetDto))]
    //[ProducesResponseType(StatusCodes.Status404NotFound)]
    //public async Task<IActionResult> GetUser([FromRoute] int id, CancellationToken cancellationToken)
    //{
    //    try
    //    {
    //        var result = await _userService.GetUser(id, cancellationToken);
    //        return Ok(result);
    //    }

    //    catch (InvalidOperationException e) 
    //    {
    //        return NotFound(e.Message);
    //    }
    //}

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