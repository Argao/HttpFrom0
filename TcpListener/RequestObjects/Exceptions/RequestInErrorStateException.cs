namespace TcpListener.RequestObjects.Exceptions;

public class RequestInErrorStateException : Exception
{
    private const string DefaultMessage = "Request is in error state";

    public RequestInErrorStateException() : base(DefaultMessage)
    {
    }

    public RequestInErrorStateException(string? message) : base(message ?? DefaultMessage)
    {
    }

    public RequestInErrorStateException(string? message, Exception? innerException) 
        : base(message ?? DefaultMessage, innerException)
    {
    }
}