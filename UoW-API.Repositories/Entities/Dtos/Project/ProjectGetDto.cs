using UoW_API.Repositories.Entities.Dtos.User;
using System;
using UoW_API.Repositories.Enums;
namespace UoW_API.Repositories.Entities.Dtos.Project;

public record ProjectGetDto(int Id, string Name, string Description, CurrentState State,
    DateTimeOffset From, DateTimeOffset To, List<UserGetDto>? Users);
