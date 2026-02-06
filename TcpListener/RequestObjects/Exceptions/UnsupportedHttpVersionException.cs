namespace TcpListener.RequestObjects.Exceptions;

public class UnsupportedHttpVersionException : Exception
{
    private const string DefaultMessage = "Unsuported HTTP version";

    public UnsupportedHttpVersionException() : base(DefaultMessage)
    {
    }

    public UnsupportedHttpVersionException(string? message) : base(message ?? DefaultMessage)
    {
    }

    public UnsupportedHttpVersionException(string? message, Exception? innerException) 
        : base(message ?? DefaultMessage, innerException)
    {
    }
}