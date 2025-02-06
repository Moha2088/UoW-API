

namespace UoW_API.Repositories.Entities;

public class User
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? ImageURL { get; set; }

    public int? ProjectId { get; set; }

    public Project? Project { get; set; }
}