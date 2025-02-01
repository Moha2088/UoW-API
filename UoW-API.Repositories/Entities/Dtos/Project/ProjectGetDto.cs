using UoW_API.Repositories.Entities.Dtos.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UoW_API.Repositories.Entities.Dtos.Project;
public class ProjectGetDto
{
    public int Id { get; set; }

    public int Name { get; set; }

    public string Description { get; set; } = null!;

    public List<UserGetDto>? Users { get; set; }
}
