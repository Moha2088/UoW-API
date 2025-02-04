namespace UoW_API.Repositories.Entities.Dtos.Project;

public record ProjectCreateDto(string Name, DateTimeOffset From, DateTimeOffset To);