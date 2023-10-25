namespace Social.Infrastructure.Exceptions;

public class UserExistException : Exception
{
    public UserExistException(string message) : base(message)
    {
        
    }
}