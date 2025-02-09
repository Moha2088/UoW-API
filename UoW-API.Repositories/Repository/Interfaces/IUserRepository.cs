using UoW_API.Repositories.Entities.Dtos.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UoW_API.Repositories.Entities;

namespace UoW_API.Repositories.Repository.Interfaces;
public interface IUserRepository : IGenericRepository<User>
{
    Task UploadImageAsync(int id, string localFilePath, CancellationToken cancellationToken);


    //Task<UserGetDto> CreateUser(UserCreateDto dto);
    //Task UploadImageAsync(int id, string localFilePath, CancellationToken cancellationToken);
    //Task<IEnumerable<UserGetDto>> GetUsers(CancellationToken cancellationToken);
    //Task<UserGetDto> GetUser(int id, CancellationToken cancellationToken);
    //Task DeleteUser(int id, CancellationToken cancellationToken);
}
