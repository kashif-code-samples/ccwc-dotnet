using ccwc;
using FluentAssertions;

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
    [InlineData("emptyfile.txt", 0, 0, 0)]
    [InlineData("lorem_ipsum.txt", 13, 109, 772)]
    [InlineData("manyemptylines.txt", 100, 0, 100)]
    [InlineData("notrailingnewline.txt", 1, 1, 2)]
    [InlineData("onelongemptyline.txt", 1, 0, 10001)]
    [InlineData("onelongword.txt", 1, 1, 10001)]
    [InlineData("UTF_8_test.txt", 303, 2119, 23025)]
    [InlineData("UTF_8_weirdchars.txt", 25, 87, 513)]
    [InlineData("test.txt", 7145, 58164, 342190)]
    public void Count_WithShowLinesWordsAndBytes_ShouldCountLinesWordsAndBytes(string file, ulong expectedLines, ulong expectedWords, ulong expectedBytes)
    {
        var sut = new Counter(new Settings { ShowLines = true, ShowWords = true, ShowBytes = true });

        var countable = new FileCountable(file);

        // Act
        var wordCount = sut.Count(countable);

        // Assert
        wordCount.Lines.Should().Be(expectedLines);
        wordCount.Words.Should().Be(expectedWords);
        wordCount.Bytes.Should().Be(expectedBytes);
    }

    [Theory]
    [InlineData("emptyfile.txt", 0)]
    [InlineData("lorem_ipsum.txt", 772)]
    [InlineData("manyemptylines.txt", 100)]
    [InlineData("notrailingnewline.txt", 2)]
    [InlineData("onelongemptyline.txt", 10001)]
    [InlineData("onelongword.txt", 10001)]
    [InlineData("test.txt", 339292)]
    public void Count_GivenShowChars_ShouldCountChars(string file, ulong expectedChars)
    {
        // Arrange
        var sut = new Counter(new Settings { ShowChars = true });

        var countable = new FileCountable(file);

        // Act
        var wordCount = sut.Count(countable);

        // Assert
        wordCount.Chars.Should().Be(expectedChars);
    }
}
