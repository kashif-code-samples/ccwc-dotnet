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
        
        var fileArgument = new Argument<string>();

        var rootCommand = new RootCommand
        {
            countBytesOption,
            countLinesOption,
            fileArgument
        };

        rootCommand.SetHandler(
            (countBytes, countLines, fileArgumentValue) =>
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
            },
            countBytesOption, countLinesOption, fileArgument);

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
}

record Counts(long ByteCount);