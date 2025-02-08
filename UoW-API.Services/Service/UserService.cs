using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using UoW_API.Repositories.Entities;
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
    private readonly IMapper _mapper;
    private const string _getUsersCachingKey = "GET_USERS";

    public UserService(IUnitOfWork unitOfWork, IRedisCacheService cacheService, ILogger<UserService> logger, IMapper mapper) =>
        (_unitOfWork, _cacheService, _logger, _mapper) = (unitOfWork, cacheService, logger, mapper);


    public async Task CreateUser(User user, CancellationToken cancellationToken)
    {
        _unitOfWork.UserRepository.Create(user, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteUser(int id, CancellationToken cancellationToken)
    {
        await _unitOfWork.UserRepository.Delete(id, cancellationToken);
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
            var dbUser = await _unitOfWork.UserRepository.Get(id, cancellationToken);
            var dto = _mapper.Map<UserGetDto>(dbUser);
            _cacheService.Set(id.ToString(), dto);
            return dto;
        }

        catch (InvalidOperationException)
        {
            throw;
        }
    }

    public async Task UploadImageAsync(int id, string localFilePath, CancellationToken cancellationToken)
    {
        try
        {
            await _unitOfWork.UserRepository.UploadImageAsync(id, localFilePath, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        catch (InvalidOperationException)
        {
            throw;
        }
    }

    public async Task<IEnumerable<UserGetDto>> GetUsers(CancellationToken cancellationToken)
    {
        var cachedUsers = _cacheService.Get<List<UserGetDto>>(_getUsersCachingKey);

        if (cachedUsers is not null)
        {
            _logger.LogInformation("Retrieving users from cache");
            return cachedUsers;
        }

        var dbUsers = await _unitOfWork.UserRepository.GetAll(cancellationToken);

        var dto = _mapper.Map<IEnumerable<UserGetDto>>(dbUsers);
        if (dto is not null)
        {
            _cacheService.Set(_getUsersCachingKey, dto);
        }

        return dto!;
    }
}
