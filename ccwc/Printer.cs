namespace ccwc;

public class Printer
{
    private readonly Settings _settings;

    public readonly TextWriter _writer;
    
    public Printer(TextWriter writer, Settings settings)
    {
        _writer = writer;
        _settings = settings;
    }

    public void PrintStats(WordCount wordCount, string title)
    {
        if (_settings.ShowLines)
        {
            _writer.Write($"{wordCount.Lines} ");
        }

        if (_settings.ShowWords)
        {
            _writer.Write($"{wordCount.Words} ");
        }

        if (_settings.ShowChars)
        {
            _writer.Write($"{wordCount.Chars} ");
        }

        if (_settings.ShowBytes)
        {
            _writer.Write($"{wordCount.Bytes} ");
        }

        _writer.WriteLine(title);
    }
}
