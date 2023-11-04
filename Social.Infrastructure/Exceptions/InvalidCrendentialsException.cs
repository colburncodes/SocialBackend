namespace Social.Infrastructure.Exceptions;

public class InvalidCrendentialsException : Exception
{
    public InvalidCrendentialsException(string message) : base(message) {}
}