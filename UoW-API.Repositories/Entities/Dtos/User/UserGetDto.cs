using UoW_API.Repositories.Entities.Dtos.Project;

namespace UoW_API.Repositories.Entities.Dtos.User;

public record UserGetDto(int Id, string Name, string ImageURL, int ProjectId, ProjectGetDto Project);