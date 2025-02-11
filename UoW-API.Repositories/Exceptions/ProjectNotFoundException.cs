

using UoW_API.Repositories.Entities;

namespace UoW_API.Repositories.Exceptions;

/// <summary>
/// Exception to be thrown when a <see cref="Project"/> object can't be found by the provided id
/// </summary>
[Serializable]
public class ProjectNotFoundException : Exception
{
    public ProjectNotFoundException() { }

    public ProjectNotFoundException(string message) : base(message) { }

    public ProjectNotFoundException(string message, Exception innerException) : base(message, innerException) { }
}
