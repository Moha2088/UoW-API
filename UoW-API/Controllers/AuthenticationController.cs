using UoW_API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using UoW_API.Repositories.Entities.Dtos;

namespace UoW_API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthenticationService _authService;

    public AuthenticationController(IAuthenticationService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] AuthenticateUserDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _authService.AuthenticateUser(dto, cancellationToken);
            return Ok(result);
        }

        catch (ArgumentNullException)
        {
            return Unauthorized();
        }
    }
}