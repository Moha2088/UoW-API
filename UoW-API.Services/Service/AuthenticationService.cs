using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using UoW_API.Repositories.Entities;
using UoW_API.Repositories.Entities.Dtos;
using UoW_API.Repositories.UnitOfWork.Interfaces;
using UoW_API.Services.Interfaces;

namespace UoW_API.Services.Service;
public class AuthenticationService : IAuthenticationService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IConfiguration _config;

    public AuthenticationService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
    }

    public async Task<string> AuthenticateUser(AuthenticateUserDto dto, CancellationToken cancellationToken)
    {
        var users = await _unitOfWork.UserRepository.GetAll(cancellationToken);

        if (!users.Any(user => user.Email.Equals(dto.Email) && user.Password.Equals(dto.Password))) 
        {
            throw new ArgumentNullException("User not found!");
        }

        var user = users.SingleOrDefault(user => user.Email.Equals(dto.Email) && user.Password.Equals(dto.Password));
        var token = GenerateToken(user!);
        return token;
    }

    public string GenerateToken(User user)
    {
        var issuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWTSettings:Key"]));
        var signingCredentials = new SigningCredentials(issuerSigningKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _config["JWTSettings:Issuer"],
            audience: _config["JWTSettings:Audience"],
            claims: new List<Claim>
            {
                    new("userId", user.Id.ToString()),
            },
            expires: DateTime.Now.AddHours(1),
            signingCredentials: signingCredentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
