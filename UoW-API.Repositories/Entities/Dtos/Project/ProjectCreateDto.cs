namespace UoW_API.Repositories.Entities.Dtos.Project;

public class ProjectCreateDto
{
    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public DateTimeOffset From { get; set; }

    public DateTimeOffset To { get; set; }
}