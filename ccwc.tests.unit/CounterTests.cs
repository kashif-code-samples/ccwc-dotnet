using ccwc;
using FluentAssertions;
using Moq;

namespace cwcc.tests.unit;

public class CounterTests
{
    [Theory]
    [InlineData("emptyfile.txt", 0)]
    [InlineData("lorem_ipsum.txt", 13)]
    [InlineData("manyemptylines.txt", 100)]
    [InlineData("notrailingnewline.txt", 1)]
    [InlineData("onelongemptyline.txt", 1)]
    [InlineData("onelongword.txt", 1)]
    [InlineData("UTF_8_test.txt", 303)]
    [InlineData("UTF_8_weirdchars.txt", 25)]
    [InlineData("test.txt", 7145)]
    public void Count_WithShowLines_ShouldCountLines(string file, ulong expectedLines)
    {
        var sut = new Counter(new Settings { ShowLines = true });

        var countable = new FileCountable(file);

        // Act
        var wordCount = sut.Count(countable);

        // Assert
        wordCount.Lines.Should().Be(expectedLines);
    }

    [Theory]
    [InlineData("emptyfile.txt", 0)]
    [InlineData("lorem_ipsum.txt", 109)]
    [InlineData("manyemptylines.txt", 0)]
    [InlineData("notrailingnewline.txt", 1)]
    [InlineData("onelongemptyline.txt", 0)]
    [InlineData("onelongword.txt", 1)]
    [InlineData("UTF_8_test.txt", 2119)]
    [InlineData("UTF_8_weirdchars.txt", 87)]
    [InlineData("test.txt", 58164)]
    public void Count_WithShowWords_ShouldCountWords(string file, ulong expectedWords)
    {
        var sut = new Counter(new Settings { ShowWords = true });

        var countable = new FileCountable(file);

        // Act
        var wordCount = sut.Count(countable);

        // Assert
        wordCount.Words.Should().Be(expectedWords);
    }

    [Theory]
    [InlineData("emptyfile.txt", 0)]
    [InlineData("lorem_ipsum.txt", 772)]
    [InlineData("manyemptylines.txt", 100)]
    [InlineData("notrailingnewline.txt", 2)]
    [InlineData("onelongemptyline.txt", 10001)]
    [InlineData("onelongword.txt", 10001)]
    [InlineData("UTF_8_test.txt", 23025)]
    [InlineData("UTF_8_weirdchars.txt", 513)]
    [InlineData("test.txt", 342190)]
    public void Count_WithShowBytes_ShouldCountBytes(string file, ulong expectedBytes)
    {
        var sut = new Counter(new Settings { ShowBytes = true });

        var countable = new FileCountable(file);

        // Act
        var wordCount = sut.Count(countable);

        // Assert
        wordCount.Bytes.Should().Be(expectedBytes);
    }

    [Theory]
    [InlineData("emptyfile.txt", 0)]
    [InlineData("lorem_ipsum.txt", 772)]
    [InlineData("manyemptylines.txt", 100)]
    [InlineData("notrailingnewline.txt", 2)]
    [InlineData("onelongemptyline.txt", 10001)]
    [InlineData("onelongword.txt", 10001)]
    [InlineData("test.txt", 342190)]
    [InlineData("The Project Gutenberg eBook of The Art of War", 46)]
    public void Count_GivenShowChars_ShouldCountChars(string text, ulong expectedChars)
    {
        // Arrange
        var sut = new Counter(new Settings { ShowChars = true });

        var stream = GetStreamFromString(text);
        var countable = new Mock<ICountable>();
        countable.Setup(x => x.Stream).Returns(stream);

        // Act
        var wordCount = sut.Count(countable.Object);

        // Assert
        wordCount.Chars.Should().Be(expectedChars);
    }

    private static MemoryStream GetStreamFromString(string s)
    {
        var stream = new MemoryStream();
        var writer = new StreamWriter(stream);
        writer.Write(s);
        writer.Flush();
        stream.Position = 0;
        return stream;
    }
}
