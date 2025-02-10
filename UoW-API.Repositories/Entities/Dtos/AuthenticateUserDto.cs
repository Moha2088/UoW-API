using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UoW_API.Repositories.Entities.Dtos;

public record AuthenticateUserDto(string Email, string Password);
