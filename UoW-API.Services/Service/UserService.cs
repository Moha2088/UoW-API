using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UoW_API.Repositories.Entities.Dtos.User;
using UoW_API.Repositories.UnitOfWork.Interfaces;
using UoW_API.Services.Interfaces;


namespace UoW_API.Services.Service;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;

    public UserService(IUnitOfWork unitOfWork) => 
        (_unitOfWork) = unitOfWork;


    public async Task<UserGetDto> CreateUser(UserCreateDto dto, CancellationToken cancellationToken)
    {
        var dbUser = await _unitOfWork.UserRepository.CreateUser(dto, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return dbUser;
    }

    public async Task DeleteUser(int id, CancellationToken cancellationToken)
    {
        await _unitOfWork.UserRepository.DeleteUser(id, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public Task<UserGetDto> GetUser(int id, CancellationToken cancellationToken)
    {
        try
        {
            return _unitOfWork.UserRepository.GetUser(id, cancellationToken);
        }

        catch (InvalidOperationException) 
        {
            throw;
        }
    }

    public Task<IEnumerable<UserGetDto>> GetUsers(CancellationToken cancellationToken)
    {
        return _unitOfWork.UserRepository.GetUsers(cancellationToken);
    }
}
