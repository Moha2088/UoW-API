using UoW_API.Repositories.Entities.Dtos.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UoW_API.Repositories.Repository.Interfaces;
public interface IUserRepository
{
    Task<UserGetDto> CreateUser(UserCreateDto dto);
    Task<IEnumerable<UserGetDto>> GetUsers();
    Task<UserGetDto> GetUser(int id);
    Task DeleteUser(int id);
}
