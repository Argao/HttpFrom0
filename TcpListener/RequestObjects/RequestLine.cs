using TcpListener.RequestObjects.Exceptions;

namespace TcpListener.RequestObjects;

public class RequestLine
{
    public string Method { get; set;}
    public string RequestTarget { get; set;}
    public string HttpVersion { get; set;}
    
    public RequestLine(string method, string requestTarget, string httpVersion)
    {
        Method = method;
        RequestTarget = requestTarget;
        HttpVersion = httpVersion;
        
        Validate();
    }

    private void Validate()
    {
        if (!Method.All(c => char.IsLetter(c) && char.IsUpper(c)))
        {
            throw new InvalidRequestLine("Invalid method format");
        }

        if (HttpVersion != "1.1")
        {
            throw new InvalidRequestLine("Invalid Http Version ");
        }
        
    }
}