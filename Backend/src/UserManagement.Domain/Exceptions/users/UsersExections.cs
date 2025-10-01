


namespace UserManagement.Domain.Exceptions.Users;


public class UserNotFoundException : DomainException
{
    public UserNotFoundException(int id) : base($"User with id {id} not found") { }
}

public class EmailAlreadyExistsException : DomainException
{
    public EmailAlreadyExistsException(string email) : base($"User with email {email} already exists") { }
}

public class InvalidCredentialsException : DomainException
{
    public InvalidCredentialsException() : base("Invalid email or password") { }
}

public class AccessLevelNotFoundException : DomainException
{
    public AccessLevelNotFoundException(int accessLevelId) : base($"Access level with id {accessLevelId} not found, User not created") { }
}