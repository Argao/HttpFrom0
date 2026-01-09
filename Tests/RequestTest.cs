using FluentAssertions;
using TcpListener;
using TcpListener.RequestObjects;
using TcpListener.RequestObjects.Exceptions;

namespace RequestTests;

public class RequestTest
{
    [Fact]
    public void GetRequestLine()
    {
        var request = new Request( new StreamReader("GET / HTTP/1.1\r\nHost: localhost:42069\r\nUser-Agent: curl/7.81.0\r\nAccept: */*\r\n\r\n"));

        request.Should().NotBeNull();
        request.RequestLine.Method.Should().Be("GET");
        request.RequestLine.RequestTarget.Should().Be("/");
        request.RequestLine.HttpVersion.Should().Be("1.1");
    }
    
    [Fact]
    public void GetRequestLineWithPath()
    {
        var request = new Request( new StreamReader("GET /coffee HTTP/1.1\r\nHost: localhost:42069\r\nUser-Agent: curl/7.81.0\r\nAccept: */*\r\n\r\n"));

        
        request.Should().NotBeNull();
        request.RequestLine.Method.Should().Be("GET");
        request.RequestLine.RequestTarget.Should().Be("/coffee");
        request.RequestLine.HttpVersion.Should().Be("1.1");
    }

    [Fact]
    public void InvalidNumberOfPartsInRequestLine()
    {
        FluentActions.Invoking(() =>
                new Request(new StreamReader(
                    "/coffee HTTP/1.1\r\nHost: localhost:42069\r\nUser-Agent: curl/7.81.0\r\nAccept: */*\r\n\r\n")))
            .Should()
            .Throw<BadStartLineException>();
    }
}