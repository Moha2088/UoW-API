

namespace UoW_API.Repositories.Entities;

public class Project
{
    public int Id { get; set; }

    public int Name { get; set; }

    public string Description { get; set; } = null!;

    public ICollection<User>? Users { get; set; }
}