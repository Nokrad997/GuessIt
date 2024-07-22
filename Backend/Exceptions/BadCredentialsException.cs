namespace Backend.Exceptions;

public class BadCredentialsException : Exception
{
    public BadCredentialsException()
    {
    }

    public BadCredentialsException(string msg)
        : base(msg)
    {
    }
}