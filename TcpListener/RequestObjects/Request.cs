using System.IO.Pipelines;
using System.Text;
using TcpListener.RequestObjects.Exceptions;

namespace TcpListener.RequestObjects;

public class Request
{
    public RequestLine RequestLine { get; private set;}
    public Dictionary<string,string>? Headers { get; set;}
    public byte[]? Body { get; set; }
    private ParserState State { get; set; } = ParserState.Initialized;
    private bool IsDone => State is ParserState.Done or ParserState.Error;
    private static readonly byte[] Separator = Encoding.UTF8.GetBytes("\r\n");


    private int Parse(byte[] data)
    {
        var read = 0;
        var iterate = true;
        while (iterate)
        {
            switch (State)
            {
                case ParserState.Error:
                    //throw new RequestInErrorStateException();
                    return 0;
                case ParserState.Initialized:
                    try
                    {
                        var result = ParseRequestLine(data[read..]);
                            
                        if (result.bytesConsumed == 0)
                        {
                            iterate = false;
                            break;
                        }
                        RequestLine = result.requestLine!;
                        read += result.bytesConsumed;
                        State = ParserState.Done;
                        
                    }
                    catch (Exception e)
                    {
                        State = ParserState.Error;
                        throw;
                    }
                    break;
                case ParserState.Done:
                    iterate = false;
                    break;
            }
        }
        return read;
    }
    
    public Request(Stream stream)
    {
        var buffer = new byte[1024];
        var bufferIndex = 0;

        while (!IsDone)
        {
            var n = stream.Read(buffer,bufferIndex, buffer.Length);
            
            bufferIndex += n;
            
            var readN = Parse(buffer);
            
            Buffer.BlockCopy(buffer, readN, buffer, 0, bufferIndex - readN);
            bufferIndex -= readN;
        }
    }
    
    


   
    
    

    private (RequestLine? requestLine, int bytesConsumed) ParseRequestLine(byte[] b)
    {


        ReadOnlySpan<byte> sourceSpan = b;
        ReadOnlySpan<byte> patternSpan = Separator;

        var index = sourceSpan.IndexOf(patternSpan);
        
        if (index == -1)
        {
            return (null, 0);
        }
        
        var startLine = Encoding.UTF8.GetString(b[..index]);
        
        var red = index + Separator.Length;
        
        var parts = startLine.Split(" ");

        if (parts.Length != 3)
            throw new MalformedRequestLineException("Invalid request line format");
        
        var httpParts = parts[2].Split("/"); 

        if (httpParts.Length != 2 || httpParts[0] != "HTTP" )
            throw new MalformedRequestLineException("Invalid HTTP Version format");


        return (new RequestLine(parts[0],parts[1],httpParts[1]),red);
    }
}