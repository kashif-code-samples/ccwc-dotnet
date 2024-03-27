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

        var wordCount = new WordCount();

        var readBytes = stream.Read(buffer, 0, BufferSize);
        while (readBytes > 0)
        {
            wordCount.Bytes += (ulong)readBytes;

            if (_settings.ShowChars || _settings.ShowLines)
            {
                if (_settings.ShowChars)
                {
                    wordCount.Chars += (ulong)GetDecoder().GetCharCount(buffer, 0, readBytes);
                }

                if (_settings.ShowLines)
                {
                    var bytes = buffer[..readBytes];
                    wordCount.Lines += (ulong)bytes.Count(x => x == (byte)'\n');
                }
            }
            
            readBytes = stream.Read(buffer, 0, BufferSize);
        }

        return wordCount;
    }

    private WordCount WordCountFromStreamSpecialized(Stream reader)
    {
        var decoder = GetDecoder();

        var buffer = new byte[BufferSize];        
        var charBuffer = new char[BufferSize];

        var wordCount = new WordCount();

        var inWord = false;

        var readBytes = reader.Read(buffer, 0, BufferSize);
        while (readBytes > 0)
        {
            wordCount.Bytes += (ulong)readBytes;

            var readChars = decoder.GetChars(buffer, 0, readBytes, charBuffer, 0);
            wordCount.Chars += (ulong)readChars;

            for (var i = 0; i < readChars; i++)
            {
                var ch = charBuffer[i];
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

            readBytes = reader.Read(buffer, 0, BufferSize);
        }

        return wordCount;
    }

    private Decoder GetDecoder()
    {
        return Encoding.UTF8.GetDecoder();
    }
}
