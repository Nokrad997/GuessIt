namespace Backend.Exceptions;

public class NoTokenProvidedException : Exception
{
    public NoTokenProvidedException()
    {
    }
    public NoTokenProvidedException(string msg)
        : base(msg)
    {
    }
}