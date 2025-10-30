namespace Models.Exceptions;

public class ReferenceNumberNotFoundException : Exception
{
    public ReferenceNumberNotFoundException()
    {
    }

    public ReferenceNumberNotFoundException(string message)
        : base(message)
    {
    }

    public ReferenceNumberNotFoundException(string message, Exception inner)
        : base(message, inner)
    {
    }
}