using TcpListener.RequestObjects.Exceptions;

namespace TcpListener.RequestObjects;

public class Request
{
    public RequestLine RequestLine { get; set;}
    public Dictionary<string,string>? Headers { get; set;}
    public byte[]? Body { get; set; }
    
    public Request(Stream stream)
    {
        var reader = new StreamReader(stream);
        RequestLine = ParseRequestLine(reader.ReadToEnd());
    }



    private RequestLine ParseRequestLine(string b)
    {
        var lines = b.Split("\r\n");

        var requestLineItens = lines[0].Split(" ");

        if (requestLineItens.Length != 3)
            throw new InvalidRequestLine("Invalid request line format");
        
        var httpParts = requestLineItens[2].Split("/");

        if (httpParts.Length != 2 || httpParts[0] != "HTTP" )
            throw new InvalidRequestLine("Invalid HTTP Version format");


        return new RequestLine(requestLineItens[0],requestLineItens[1],httpParts[1]);
    }
}