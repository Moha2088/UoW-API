

using UoW_API.Repositories.Enums;

namespace UoW_API.Repositories.Entities;

public class Project
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; } = null!;

    public CurrentState State { get; set; }

    public DateTimeOffset From { get; set; }
    
    public DateTimeOffset To { get; set; }

    public ICollection<User>? Users { get; set; }
}