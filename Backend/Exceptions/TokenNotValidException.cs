namespace Backend.Exceptions;

public class TokenNotValidException : Exception
{
    public TokenNotValidException()
    {
    }
    
    public TokenNotValidException(string msg)
        : base(msg)
    {
    }
}