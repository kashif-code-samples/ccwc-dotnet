using System.CommandLine;
using System.CommandLine.Binding;

namespace WordCount.App;

public class Options
{
    public bool PrintBytes { get; set; }
    public bool PrintLines { get; set; }
    public bool PrintWords { get; set; }
}

public class OptionsBinder : BinderBase<Options>
{
    private readonly Option<bool> _bytesOption;

    private readonly Option<bool> _linesOption;
    private readonly Option<bool> _wordsOption;

    public OptionsBinder(
        Option<bool> bytesOption,
        Option<bool> linesOption,
        Option<bool> wordsOption)
    {
        _bytesOption = bytesOption;
        _linesOption = linesOption;
        _wordsOption = wordsOption;
    }

    protected override Options GetBoundValue(BindingContext bindingContext) =>
        new Options
        {
            PrintBytes = bindingContext.ParseResult.GetValueForOption(_bytesOption),
            PrintLines = bindingContext.ParseResult.GetValueForOption(_linesOption),
            PrintWords = bindingContext.ParseResult.GetValueForOption(_wordsOption),
        };
}
