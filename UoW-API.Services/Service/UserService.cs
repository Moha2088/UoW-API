using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using UoW_API.Repositories.Entities.Dtos.User;
using UoW_API.Repositories.Repository.Caching.Interfaces;
using UoW_API.Repositories.UnitOfWork.Interfaces;
using UoW_API.Services.Interfaces;


namespace UoW_API.Services.Service;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRedisCacheService _cacheService;
    private readonly ILogger<UserService> _logger;
    private const string _projectCachingKey = "GET_PROJECTS";

    public UserService(IUnitOfWork unitOfWork, IRedisCacheService cacheService, ILogger<UserService> logger) =>
        (_unitOfWork, _cacheService, _logger) = (unitOfWork, cacheService, logger);


    public async Task<UserGetDto> CreateUser(UserCreateDto dto, CancellationToken cancellationToken)
    {
        var dbUser = await _unitOfWork.UserRepository.CreateUser(dto);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return dbUser;
    }

    public async Task DeleteUser(int id, CancellationToken cancellationToken)
    {
        await _unitOfWork.UserRepository.DeleteUser(id, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task<UserGetDto> GetUser(int id, CancellationToken cancellationToken)
    {

        var cachedUser = _cacheService.Get<UserGetDto>(id.ToString());

        if (cachedUser is not null)
        {
            _logger.LogInformation("Retrieving data from cache!");
            return cachedUser;
        }

        try
        {
            var dbUser = await _unitOfWork.UserRepository.GetUser(id, cancellationToken);
            _cacheService.Set<UserGetDto>(id.ToString(), dbUser);
            return dbUser;
        }

        catch (InvalidOperationException)
        {
            throw;
        }
    }

    public async Task<IEnumerable<UserGetDto>> GetUsers(CancellationToken cancellationToken)
    {
        var cachedUsers = _cacheService.Get<List<UserGetDto>>(_projectCachingKey);

        if (cachedUsers is not null)
        {
            _logger.LogInformation("Retrieving users from cache");
            return cachedUsers;
        }

        var dbUsers = await _unitOfWork.UserRepository.GetUsers(cancellationToken);

        if (dbUsers is not null)
        {
            _cacheService.Set(_projectCachingKey, dbUsers);
        }

        return dbUsers!;
    }
}
