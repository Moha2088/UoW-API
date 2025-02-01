using UoW_API.Repositories.Data;
using UoW_API.Repositories.Entities;
using UoW_API.Repositories.Entities.Dtos.User;
using UoW_API.Repositories.Repository.Interfaces;

namespace UoW_API.Repositories.Repository;

public class UserRepository : IUserRepository
{
    private readonly DataContext _context;

    public UserRepository(DataContext context)
    {
        _context = context;
    }

    public Task<UserGetDto> CreateUser(UserCreateDto dto)
    {
        throw new NotImplementedException();
    }

    public Task DeleteUser(int id)
    {
        throw new NotImplementedException();
    }

    public Task<UserGetDto> GetUser(int id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<UserGetDto>> GetUsers()
    {
        throw new NotImplementedException();
    }
}
