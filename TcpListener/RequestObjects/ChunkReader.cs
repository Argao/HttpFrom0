using System.Text;

namespace TcpListener.RequestObjects;

/// <summary>
/// Lê até len(p) ou numBytesPerRead bytes da string por chamada.
/// Útil para simular leitura de número variável de bytes por chunk de uma conexão de rede.
/// Equivalente ao chunkReader do Go.
/// </summary>
public class ChunkReader : Stream
{
    private readonly byte[] _data;
    private readonly int _numBytesPerRead;
    private int _pos;

    public ChunkReader(string data, int numBytesPerRead,int pos = 0)
    {
        _data = Encoding.UTF8.GetBytes(data);
        _numBytesPerRead = numBytesPerRead;
        _pos = pos;
    }

    /// <summary>
    /// Lê até len(buffer) ou numBytesPerRead bytes da string por chamada.
    /// Equivalente a: func (cr *chunkReader) Read(p []byte) (n int, err error)
    /// </summary>
    public override int Read(byte[] buffer, int offset, int count)
    {
        // if cr.pos >= len(cr.data) { return 0, io.EOF }
        if (_pos >= _data.Length)
        {
            return 0; // EOF em .NET é indicado por retornar 0
        }

        // endIndex := cr.pos + cr.numBytesPerRead
        int endIndex = _pos + _numBytesPerRead;

        // if endIndex > len(cr.data) { endIndex = len(cr.data) }
        if (endIndex > _data.Length)
        {
            endIndex = _data.Length;
        }

        // Também limita pelo tamanho do buffer solicitado (equivalente a len(p) no Go)
        int bytesToRead = endIndex - _pos;
        if (bytesToRead > count)
        {
            bytesToRead = count;
        }

        // n = copy(p, cr.data[cr.pos:endIndex])
        Array.Copy(_data, _pos, buffer, offset, bytesToRead);

        // cr.pos += n
        _pos += bytesToRead;

        // return n, nil
        return bytesToRead;
    }

    // Propriedades obrigatórias de Stream (somente leitura)
    public override bool CanRead => true;
    public override bool CanSeek => false;
    public override bool CanWrite => false;
    public override long Length => _data.Length;
    public override long Position
    {
        get => _pos;
        set => throw new NotSupportedException();
    }

    public override void Flush() { }
    public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException();
    public override void SetLength(long value) => throw new NotSupportedException();
    public override void Write(byte[] buffer, int offset, int count) => throw new NotSupportedException();
}

