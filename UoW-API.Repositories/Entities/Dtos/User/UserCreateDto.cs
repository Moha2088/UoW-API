using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UoW_API.Repositories.Entities.Dtos.User;
public class UserCreateDto
{
    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;
}
