using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clean_Project_Backend.Repositories.Entities.Dtos.User;
public class UserCreateDto
{
    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;
}
