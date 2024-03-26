using System.CommandLine;
using ccwc;

var bytesOption = new Option<bool>(
    name: "--bytes",
    description: "print the byte counts",
    getDefaultValue: () => false);
bytesOption.AddAlias("-c");

var charsOption = new Option<bool>(
    name: "--chars",
    description: "print the character counts",
    getDefaultValue: () => false);
bytesOption.AddAlias("-m");

var linesOption = new Option<bool>(
    name: "--lines",
    description: "print the newline counts",
    getDefaultValue: () => false);
linesOption.AddAlias("-l");

var wordsOption = new Option<bool>(
    name: "--words",
    description: "print the word counts",
    getDefaultValue: () => false);
wordsOption.AddAlias("-w");

var maxLineLengthOption = new Option<bool>(
    name: "--max-line-length",
    description: "print the maximum display width",
    getDefaultValue: () => false);
wordsOption.AddAlias("-L");

var fileArgument = new Argument<string>();

var rootCommand = new RootCommand
{
    bytesOption,
    charsOption,
    linesOption,
    wordsOption,
    fileArgument
};

rootCommand.SetHandler((settings, fileArgumentValue) =>
    {
        var countable = CountableFactory.Create(fileArgumentValue);
        var counter = new Counter(settings);

        var wordCount = counter.Count(countable);

        var printer = new Printer(Console.Out, settings);
        printer.PrintStats(wordCount, countable.File);
    },
    new SettingsBinder(bytesOption, charsOption, linesOption, wordsOption),
    fileArgument);

await rootCommand.InvokeAsync(args);