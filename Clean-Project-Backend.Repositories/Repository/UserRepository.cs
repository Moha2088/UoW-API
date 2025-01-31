using Clean_Project_Backend.Repositories.Entities;
using Clean_Project_Backend.Repositories.Entities.Dtos.User;
using Clean_Project_Backend.Repositories.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clean_Project_Backend.Repositories.Repository;

public class UserRepository : IUserRepository
{
    public UserRepository()
    {
        
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
