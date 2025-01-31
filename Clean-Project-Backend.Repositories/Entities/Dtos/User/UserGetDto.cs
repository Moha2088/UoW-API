using Clean_Project_Backend.Repositories.Entities.Dtos.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clean_Project_Backend.Repositories.Entities.Dtos.User;
public class UserGetDto
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string ImageURL { get; set; } = null!;

    public int ProjectId { get; set; }

    public ProjectGetDto Project { get; set; }
}
