using System.Runtime.Serialization;

namespace TcpListener.RequestObjects.Exceptions;

public class MalformedRequestLineException : Exception
{
    private const string DefaultMessage = "Malformed request-line";

    public MalformedRequestLineException() : base(DefaultMessage)
    {
    }

    public MalformedRequestLineException(string? message) : base(message ?? DefaultMessage)
    {
    }

    public MalformedRequestLineException(string? message, Exception? innerException) 
        : base(message ?? DefaultMessage, innerException)
    {
    }
}