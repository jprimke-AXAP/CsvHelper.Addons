using System.Globalization;
using CsvHelper.Configuration;
using CsvHelper.FixedLengthParser;
using FluentAssertions;
using Xunit;

namespace CsvHelper.FixedLengthParser.Specs.Parser;

public class FixedLengthParserBlankLineSpec
{
    [Fact]
    public void Throws_when_line_is_blank()
    {
        // Arrange
        var tempPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".txt");
        File.WriteAllText(tempPath, "\n");

        var options = new FixedLengthOptions(new Dictionary<int, Range>
        {
            { 0, new Range(0, 5) },
        });

        var cfg = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            TrimOptions = TrimOptions.Trim,
        };

        using (var parser = new FixedLengthParser(tempPath, options, cfg))
        {
            // Act
            Action act = () => parser.Read();

            // Assert
            act.Should().Throw<IndexOutOfRangeException>();
        }

        // Cleanup
        File.Delete(tempPath);
    }
}
