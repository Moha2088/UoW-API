

namespace Clean_Project_Backend.Repositories.Entities;

public class User
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string ImageURL { get; set; } = null!;

    public int ProjectId { get; set; }

    public Project? Project { get; set; }
}