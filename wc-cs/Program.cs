using System.CommandLine;

class Program
{
    static async Task Main(string[] args)
    {
        var countBytesOption = new Option<bool>(
            name: "--bytes",
            description: "print the byte counts",
            getDefaultValue: () => true);
        countBytesOption.AddAlias("-c");

        var fileArgument = new Argument<string>();

        var rootCommand = new RootCommand
        {
            countBytesOption,
            fileArgument
        };

        rootCommand.SetHandler(
            (byteCountOption, fileArgumentValue) =>
            {
                Console.WriteLine($"{byteCountOption} {fileArgumentValue}");
            },
            countBytesOption, fileArgument);

        await rootCommand.InvokeAsync(args);
    }
}
