using System.Text;
using FluentAssertions;
using TcpListener;
using TcpListener.RequestObjects;
using TcpListener.RequestObjects.Exceptions;

namespace RequestTests;

public class RequestTest
{
    private static Stream StringToStream(string text)
    {
        return new MemoryStream(Encoding.UTF8.GetBytes(text));
    }
    
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    [InlineData(6)]
    [InlineData(7)]
    [InlineData(8)]
    [InlineData(15)]
    public void GetRequestLine(int n)
    {
        var text = "GET / HTTP/1.1\r\nHost: localhost:42069\r\nUser-Agent: curl/7.81.0\r\nAccept: */*\r\n\r\n";
        var request = new Request(StringToStream(text));

        request.Should().NotBeNull();
        request.RequestLine.Method.Should().Be("GET");
        request.RequestLine.RequestTarget.Should().Be("/");
         request.RequestLine.HttpVersion.Should().Be("1.1");
    }
    
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    [InlineData(6)]
    [InlineData(7)]
    [InlineData(8)]
    [InlineData(15)]
    public void GetRequestLineWithPath(int n)
    {
        var text = "GET /coffee HTTP/1.1\r\nHost: localhost:42069\r\nUser-Agent: curl/7.81.0\r\nAccept: */*\r\n\r\n";
        
        var request = new Request(StringToStream(text));
        
        request.Should().NotBeNull();
        request.RequestLine.Method.Should().Be("GET");
        request.RequestLine.RequestTarget.Should().Be("/coffee");
        request.RequestLine.HttpVersion.Should().Be("1.1");
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    [InlineData(6)]
    [InlineData(7)]
    [InlineData(8)]
    [InlineData(15)]
    public void InvalidNumberOfPartsInRequestLine(int n)
    {
        var text = "/coffee HTTP/1.1\r\nHost: localhost:42069\r\nUser-Agent: curl/7.81.0\r\nAccept: */*\r\n\r\n";
      
        
        FluentActions.Invoking(() => new Request(StringToStream(text)))
            .Should()
            .Throw<InvalidRequestLine>();
    }
}