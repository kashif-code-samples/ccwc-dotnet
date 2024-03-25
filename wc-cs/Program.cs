using System.CommandLine;

class Program
{
    static async Task Main(string[] args)
    {
        var countBytesOption = new Option<bool>(
            name: "--bytes",
            description: "print the byte counts",
            getDefaultValue: () => false);
        countBytesOption.AddAlias("-c");

        var countLinesOption = new Option<bool>(
            name: "--lines",
            description: "print the newline counts",
            getDefaultValue: () => false);
        countLinesOption.AddAlias("-l");

        var countWordsOption = new Option<bool>(
            name: "--words",
            description: "print the word counts",
            getDefaultValue: () => false);
        countWordsOption.AddAlias("-w");
        
        var fileArgument = new Argument<string>();

        var rootCommand = new RootCommand
        {
            countBytesOption,
            countLinesOption,
            countWordsOption,
            fileArgument
        };

        rootCommand.SetHandler(
            (countBytes, countLines, countWords, fileArgumentValue) =>
            {
                if (countBytes)
                {
                    var byteCount = GetByteCount(fileArgumentValue);
                    Console.WriteLine($"{byteCount} {fileArgumentValue}");
                }

                if (countLines)
                {
                    var lineCount = GetLineCount(fileArgumentValue);
                    Console.WriteLine($"{lineCount} {fileArgumentValue}");
                }

                if (countWords)
                {
                    var wordCount = GetWordCount(fileArgumentValue);
                    Console.WriteLine($"{wordCount} {fileArgumentValue}");
                }
            },
            countBytesOption, countLinesOption, countWordsOption, fileArgument);

        await rootCommand.InvokeAsync(args);
    }

    static long GetByteCount(string filename)
    {
        var fs = new FileStream(filename, FileMode.Open, FileAccess.Read);
        return fs.Length;
    }

    static long GetLineCount(string filename)
    {
        var reader = new StreamReader(new FileStream(filename, FileMode.Open, FileAccess.Read));
        
        var lineCount = 0L;
        while (reader.ReadLine() != null)
        {
            lineCount++;
        }

        return lineCount;
    }

    static long GetWordCount(string filename)
    {
        var reader = new StreamReader(new FileStream(filename, FileMode.Open, FileAccess.Read));
        
        var wordCount = 0L;
        while (reader.Peek() != -1)
        {
            var ch = (char) reader.Read();
            var nextCh = (char) reader.Peek();
            if (!char.IsWhiteSpace(ch) && char.IsWhiteSpace(nextCh))
            {
                wordCount++;
            }
        }

        return wordCount;
    }
}

record Counts(long ByteCount);