using System.Net;
using System.Net.Sockets;
using System.Text;
using TcpListener.RequestObjects;

namespace TcpListener;

class Program
{
   

    public static async Task Main()
    {

      
      
        var listener = new System.Net.Sockets.TcpListener(IPAddress.Any,42069);
        Console.WriteLine($"Listening on {IPAddress.Any}:{42069}");
        listener.Start();

        try
        {
            while (true)
            {
                var conn = await listener.AcceptTcpClientAsync();
                await using var stream = conn.GetStream(); // NetworkStream

                var teste = new Request(stream);

                Console.WriteLine(teste.RequestLine.ToString());

                /*await foreach (var line in GetLinesChannel(stream))
                {
                    Console.WriteLine($"read: {line}");
                }*/
                conn.Close();      
            }
        }
        catch (Exception e)
        {
            listener.Stop();
            Console.WriteLine(e);
            throw;
        }
    }

    
    
    
    
    
    
    /// <summary>
    /// Lê linhas de um stream, 8 bytes por vez.
    /// O stream é fechado quando a leitura termina.
    /// Equivalente a: func getLinesChannel(f io.ReadCloser) &lt;-chan string
    /// </summary>
    private static async IAsyncEnumerable<string> GetLinesChannel(Stream stream)
    {
        await using (stream) // equivalente a "defer f.Close()" em Go
        {
            var buffer = new byte[8];
            var currentLine = new StringBuilder();

            while (true)
            {
                int bytesRead = await stream.ReadAsync(buffer);
                if (bytesRead == 0)
                {
                    // Envia conteúdo restante se houver (linha sem \n no final)
                    if (currentLine.Length > 0)
                    {
                        yield return currentLine.ToString();
                    }
                    yield break; // equivalente a "close(lines)" em Go
                }

                var data = buffer.AsMemory(0, bytesRead);

                while (data.Length > 0)
                {
                    int newlineIndex = data.Span.IndexOf((byte)'\n');

                    if (newlineIndex == -1)
                    {
                        // Nenhum \n encontrado, acumula e lê mais
                        currentLine.Append(Encoding.UTF8.GetString(data.Span));
                        break;
                    }

                    // Encontrou \n, completa a linha e envia
                    currentLine.Append(Encoding.UTF8.GetString(data.Span[..newlineIndex]));
                    if (currentLine.Length > 0)
                    {
                        yield return currentLine.ToString(); // equivalente a "lines <- line" em Go
                    }
                    currentLine.Clear();

                    // Avança após o \n
                    data = data[(newlineIndex + 1)..];
                }
            }
        }
    }
}