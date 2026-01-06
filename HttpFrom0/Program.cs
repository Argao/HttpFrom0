using System.Text;

namespace HttpFrom0;

class Program
{
    const string InputFilePath = "messages.txt";

    public static async Task Main()
    {
        try
        {
            var stream = File.OpenRead(InputFilePath);
            
            Console.WriteLine($"Reading data from {InputFilePath}");
            Console.WriteLine("=====================================");

            await foreach (var line in GetLinesChannel(stream))
            {
                Console.WriteLine($"read: {line}");
            }
        }
        catch (FileNotFoundException)
        {
            await Console.Error.WriteLineAsync($"could not open {InputFilePath}");
        }
    }

    /// <summary>
    /// Lê linhas de um stream, 8 bytes por vez.
    /// O stream é fechado quando a leitura termina.
    /// Equivalente a: func getLinesChannel(f io.ReadCloser) &lt;-chan string
    /// </summary>
    static async IAsyncEnumerable<string> GetLinesChannel(Stream stream)
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