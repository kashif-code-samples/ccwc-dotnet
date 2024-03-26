using System.Text;

namespace ccwc;

public class Counter(Settings settings)
{
    const int BufferSize = 1024;

    private readonly Settings _settings = settings;

    public WordCount Count(ICountable countable)
    {
        var stream = countable.Stream;
        
        return _settings switch
        {
            (true, false, false, false) => CountBytesFast(stream),
            (_, _, _, false) => CountBytesCharsAndLinesFast(stream),
            (_, _, _, true) => WordCountFromStreamSpecialized(stream),
        };
    }

    private WordCount CountBytesFast(Stream stream)
    {
        try
        {
            var length = (ulong) stream.Length;
            return new WordCount { Bytes = length };
        }
        catch
        {
            return CountBytesCharsAndLinesFast(stream);
        }
    }

    private WordCount CountBytesCharsAndLinesFast(Stream stream)
    {
        var buffer = new byte[BufferSize];
        Span<byte> span = buffer;

        var wordCount = new WordCount();

        var n = stream.Read(span);
        while (n > 0)
        {
            wordCount.Bytes += (ulong)n;

            if (_settings.ShowChars || _settings.ShowLines)
            {
                var bytes = span[..n];
                if (_settings.ShowChars)
                {
                    wordCount.Chars += (ulong)Encoding.UTF8.GetCharCount(bytes);
                }

                if (_settings.ShowLines)
                {
                    wordCount.Lines += (ulong)bytes.Count((byte)'\n');
                }
            }
            
            n = stream.Read(span);
        }

        return wordCount;
    }

    private WordCount WordCountFromStreamSpecialized(Stream reader)
    {
        var encoding = Encoding.UTF8;
        var decoder = encoding.GetDecoder();

        var buffer = new byte[BufferSize];        
        Span<byte> span = buffer;

        var maxCharCount = encoding.GetMaxCharCount(span.Length);
        var charBuffer = new char[maxCharCount];
        Span<char> charSpan = charBuffer;

        var wordCount = new WordCount();

        var inWord = false;

        var n = reader.Read(span);
        var nc = decoder.GetChars(span[..n], charSpan, n <= 0);
        while (nc > 0)
        {
            wordCount.Bytes += (ulong)n;
            wordCount.Chars += (ulong)nc;

            for (var i = 0; i < nc; i++)
            {
                var ch = charSpan[i];
                if (_settings.ShowWords)
                {
                    if (char.IsWhiteSpace(ch))
                    {
                        inWord = false;
                    }
                    else if (char.IsControl(ch))
                    { }
                    else if (!inWord)
                    {
                        wordCount.Words++;
                        inWord = true;
                    }
                }

                if (_settings.ShowLines && ch == '\n')
                {
                    wordCount.Lines++;
                }
            }

            n = reader.Read(span);
            nc = decoder.GetChars(span[..n], charSpan, n <= 0);
        }

        return wordCount;
    }
}
