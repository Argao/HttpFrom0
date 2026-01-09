using System.Runtime.Serialization;

namespace TcpListener.RequestObjects.Exceptions;

public class InvalidRequestLine : Exception
{
    private const string DefaultMessage = "Bad start line";

    public InvalidRequestLine() : base(DefaultMessage)
    {
    }

    public InvalidRequestLine(string? message) : base(message ?? DefaultMessage)
    {
    }

    public InvalidRequestLine(string? message, Exception? innerException) 
        : base(message ?? DefaultMessage, innerException)
    {
    }
}