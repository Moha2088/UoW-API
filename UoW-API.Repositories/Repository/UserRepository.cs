using AutoMapper;
using Microsoft.EntityFrameworkCore;
using UoW_API.Repositories.Data;
using UoW_API.Repositories.Entities;
using UoW_API.Repositories.Entities.Dtos.User;
using UoW_API.Repositories.Repository.Interfaces;

namespace UoW_API.Repositories.Repository;

public class UserRepository : IUserRepository
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public UserRepository(DataContext context, IMapper mapper) =>
        (_context, _mapper) = (context, mapper);


    public async Task<UserGetDto> CreateUser(UserCreateDto dto, CancellationToken cancellationToken)
    {
        var dbUser = _mapper.Map<User>(dto);
        _context.Users.Add(dbUser);
        return _mapper.Map<UserGetDto>(dbUser);
    }

    public async Task DeleteUser(int id, CancellationToken cancellationToken)
    {
        var dbUser = await _context.Users
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.Id.Equals(id));

        _context.Users.Remove(dbUser);
    }

    public async Task<UserGetDto> GetUser(int id, CancellationToken cancellationToken)
    {
        var dbUser = await _context.Users
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.Id.Equals(id));

        if (dbUser == null)
        {
            throw new InvalidOperationException("User not found");
        }

        return _mapper.Map<UserGetDto>(dbUser);
    }

    public async Task<IEnumerable<UserGetDto>> GetUsers(CancellationToken cancellationToken)
    {
        var dbUsers = await _context.Users
            .AsNoTracking()
            .ToListAsync();

        return _mapper.Map<List<UserGetDto>>(dbUsers);
    }
}
