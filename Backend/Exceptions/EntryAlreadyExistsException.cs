namespace Backend.Exceptions;

public class EntryAlreadyExistsException : Exception
{
    private const string DefaultMessage = "Entry already exists";
    public EntryAlreadyExistsException() : base(DefaultMessage)
    {
    }
    public EntryAlreadyExistsException(string message) : base(message)
    {
    }
}