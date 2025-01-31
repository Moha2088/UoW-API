using Clean_Project_Backend.Repositories.Entities.Dtos.User;
using Clean_Project_Backend.Repositories.UnitOfWork.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Clean_Project_Backend.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private IUnitOfWork _unitOfWork;
    private ILogger<UsersController> _logger;

    public UsersController(IUnitOfWork unitOfWork, ILogger<UsersController> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    [HttpPost]
    public async Task AddUser([FromBody] UserCreateDto dto)
    {
        

    }
}