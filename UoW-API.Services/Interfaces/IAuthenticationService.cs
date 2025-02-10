using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UoW_API.Repositories.Entities;
using UoW_API.Repositories.Entities.Dtos;

namespace UoW_API.Services.Interfaces;
public interface IAuthenticationService
{
    /// <summary>
    /// Authenticates a user
    /// </summary>
    /// <param name="dto">Data to authenticate a user</param>
    /// <returns>A token if the user exists</returns>
    public Task<string> AuthenticateUser(AuthenticateUserDto dto, CancellationToken cancellationToken);

    /// <summary>
    /// Generates a JWT Token
    /// </summary>
    /// <param name="user">The user to which the token is generated for</param>
    /// <returns>A A JWT token</returns>
    public string GenerateToken(User user);
}
