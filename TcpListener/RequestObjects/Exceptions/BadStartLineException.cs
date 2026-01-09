using System.Runtime.Serialization;

namespace TcpListener.RequestObjects.Exceptions;

public class BadStartLineException : Exception
{
    private const string DefaultMessage = "Bad start line";

    public BadStartLineException() : base(DefaultMessage)
    {
    }

    public BadStartLineException(string? message) : base(message ?? DefaultMessage)
    {
    }

    public BadStartLineException(string? message, Exception? innerException) 
        : base(message ?? DefaultMessage, innerException)
    {
    }
}