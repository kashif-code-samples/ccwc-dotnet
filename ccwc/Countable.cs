namespace ccwc;

public interface ICountable
{
    string File { get; }
    Stream Stream { get; }
}

public class StdinCountable : ICountable
{
    public string File => "";

    public Stream Stream => Console.OpenStandardInput();
}

public class FileCountable : ICountable
{
    public string File { get; }

    public Stream Stream { get; }

    public FileCountable(string file)
    {
        File = file;
        Stream = new FileStream(file, FileMode.Open, FileAccess.Read);
    }
}

public class CountableFactory
{
    public static ICountable Create(string file)
    {
        if (string.IsNullOrEmpty(file) || file == "-")
        {
            return new StdinCountable();
        }

        return new FileCountable(file);
    }
}