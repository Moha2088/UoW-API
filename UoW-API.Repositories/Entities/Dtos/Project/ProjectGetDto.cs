using UoW_API.Repositories.Entities.Dtos.User;
using System;
using UoW_API.Repositories.Enums;
namespace UoW_API.Repositories.Entities.Dtos.Project;

public class ProjectGetDto
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public CurrentState State { get; set; }

    public DateTimeOffset From { get; set; }

    public DateTimeOffset To { get; set; }

    public List<UserGetDto>? Users { get; set; }
    
}
