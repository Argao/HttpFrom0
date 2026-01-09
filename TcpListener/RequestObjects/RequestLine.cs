namespace TcpListener.RequestObjects;

public class RequestLine
{
    public required string HttpVersion { get; set;}
    public required string RequestTarget { get; set;}
    public required string Method { get; set;}
}