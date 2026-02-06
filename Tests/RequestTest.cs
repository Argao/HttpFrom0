using FluentAssertions;
using TcpListener.RequestObjects;
using TcpListener.RequestObjects.Exceptions;

namespace RequestTests;

public class RequestTest
{
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
        var reader = new ChunkReader(
            data: "GET / HTTP/1.1\r\nHost: localhost:42069\r\nUser-Agent: curl/7.81.0\r\nAccept: */*\r\n\r\n",
            numBytesPerRead: n
        );
        
        var request = new Request(reader);

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
        var reader = new ChunkReader(
            data: "GET /coffee HTTP/1.1\r\nHost: localhost:42069\r\nUser-Agent: curl/7.81.0\r\nAccept: */*\r\n\r\n",
            numBytesPerRead: n
        );
        
        var request = new Request(reader);
        
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
        var reader = new ChunkReader(
            data: "/coffee HTTP/1.1\r\nHost: localhost:42069\r\nUser-Agent: curl/7.81.0\r\nAccept: */*\r\n\r\n",
            numBytesPerRead: n
        );
        
        FluentActions.Invoking(() => new Request(reader))
            .Should()
            .Throw<MalformedRequestLineException>();
    }
}