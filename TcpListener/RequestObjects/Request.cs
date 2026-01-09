namespace TcpListener.RequestObjects;

public class Request
{
    public RequestLine RequestLine { get; set;}
    public Dictionary<string,string> Headers { get; set;}
    public byte[] Body { get; set; }
    
    public Request(StreamReader reader)
    {
    }


}