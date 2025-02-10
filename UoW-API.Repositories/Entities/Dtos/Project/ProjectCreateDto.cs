using UoW_API.Repositories.Entities.Dtos.User;
using UoW_API.Repositories.Enums;

namespace UoW_API.Repositories.Entities.Dtos.Project;

public record ProjectCreateDto(string Name, string Description,
    DateTimeOffset From, DateTimeOffset To);