

using UoW_API.Repositories.Entities;

namespace UoW_API.Repositories.Exceptions;

/// <summary>
/// Exception to be thrown when a <see cref="User"/> object can't be found by the provided id
/// </summary>
[Serializable]
public class UserNotFoundException : Exception
{
    public UserNotFoundException() { }

    public UserNotFoundException(string message) : base(message) { }

    public UserNotFoundException(string message, Exception innerException) : base(message, innerException) { }
}
