namespace Models.Exceptions;


public class BookingLimitExceededException : Exception
{
    public BookingLimitExceededException()
    {
    }

    public BookingLimitExceededException(string message)
        : base(message)
    {
    }

    public BookingLimitExceededException(string message, Exception inner)
        : base(message, inner)
    {
    }
}
