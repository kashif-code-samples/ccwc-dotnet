using System.CommandLine;
using System.CommandLine.Binding;

namespace ccwc;

public class Settings
{
    public bool ShowBytes { get; set; }
    public bool ShowChars { get; set; }
    public bool ShowLines { get; set; }
    public bool ShowWords { get; set; }

    public void Deconstruct(out bool showBytes, out bool showChars, out bool showLines, out bool showWords)
    {
        showBytes = ShowBytes;
        showChars = ShowChars;
        showLines = ShowLines;
        showWords = ShowWords;
    }
}

public class SettingsBinder : BinderBase<Settings>
{
    private readonly Option<bool> _bytesOption;
    private readonly Option<bool> _charsOption;
    private readonly Option<bool> _linesOption;
    private readonly Option<bool> _wordsOption;

    public SettingsBinder(
        Option<bool> bytesOption,
        Option<bool> charsOption,
        Option<bool> linesOption,
        Option<bool> wordsOption)
    {
        _bytesOption = bytesOption;
        _charsOption = charsOption;
        _linesOption = linesOption;
        _wordsOption = wordsOption;
    }

    protected override Settings GetBoundValue(BindingContext bindingContext)
    {
        var showBytes = bindingContext.ParseResult.GetValueForOption(_bytesOption);
        var showChars = bindingContext.ParseResult.GetValueForOption(_charsOption);
        var showLines = bindingContext.ParseResult.GetValueForOption(_linesOption);
        var showWords = bindingContext.ParseResult.GetValueForOption(_wordsOption);

        if (!showBytes && !showChars && !showLines && !showWords)
        {
            showLines = true;
            showWords = true;
            showBytes = true;
        }

        return new Settings
        {
            ShowBytes = showBytes,
            ShowChars = showChars,
            ShowLines = showLines,
            ShowWords = showWords,
        };
    }
}