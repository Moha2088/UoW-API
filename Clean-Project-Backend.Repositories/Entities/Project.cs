

namespace Clean_Project_Backend.Repositories.Entities;

public class Project
{
    public int Id { get; set; }

    public int Name { get; set; }

    public string Description { get; set; } = null!;

    public ICollection<User>? Users { get; set; }
}