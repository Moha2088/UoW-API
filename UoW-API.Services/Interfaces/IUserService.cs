using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UoW_API.Repositories.Entities;
using UoW_API.Repositories.Entities.Dtos.User;

namespace UoW_API.Services.Interfaces;
public interface IUserService
{
    Task CreateUser(UserCreateDto dto, CancellationToken cancellationToken);
    Task UploadImageAsync(int id, string localFilePath, CancellationToken cancellationToken);
    Task<IEnumerable<UserGetDto>> GetUsers(CancellationToken cancellationToken);
    Task<UserGetDto> GetUser(int id, CancellationToken cancellationToken);
    Task DeleteUser(int id, CancellationToken cancellationToken);
}
