using System.Globalization;
using CsvHelper.Configuration;
using CsvHelper.FixedLengthParser;
using FluentAssertions;
using Xunit;

namespace CsvHelper.FixedLengthParser.Specs.Parser;

public class FixedLengthParserTrimBehaviorSpec
{
    [Fact]
    public void Does_not_trim_when_trimoptions_none()
    {
        // Arrange: name has a trailing space within the fixed range
        var tempPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".txt");
        var lines = new[] { "Bill 20" }; // [0..5) => "Bill ", [5..7) => "20"
        File.WriteAllLines(tempPath, lines);

        var options = new FixedLengthOptions(new Dictionary<int, Range>
        {
            { 0, new Range(0, 5) },
            { 1, new Range(5, 7) },
        });

        var cfg = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            TrimOptions = TrimOptions.None,
        };

        using (var parser = new FixedLengthParser(tempPath, options, cfg))
        {
            // Act
            parser.Read().Should().BeTrue();

            // Assert
            parser[0].Should().Be("Bill "); // trailing space preserved
            parser[1].Should().Be("20");
        }

        // Cleanup
        File.Delete(tempPath);
    }

    [Fact]
    public void Trims_when_trimoptions_trim()
    {
        // Arrange
        var tempPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".txt");
        var lines = new[] { "Bill 20" }; // [0..5) => "Bill ", [5..7) => "20"
        File.WriteAllLines(tempPath, lines);

        var options = new FixedLengthOptions(new Dictionary<int, Range>
        {
            { 0, new Range(0, 5) },
            { 1, new Range(5, 7) },
        });

        var cfg = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            TrimOptions = TrimOptions.Trim,
        };

        using (var parser = new FixedLengthParser(tempPath, options, cfg))
        {
            // Act
            parser.Read().Should().BeTrue();

            // Assert
            parser[0].Should().Be("Bill"); // trimmed
            parser[1].Should().Be("20");
        }

        // Cleanup
        File.Delete(tempPath);
    }
}
