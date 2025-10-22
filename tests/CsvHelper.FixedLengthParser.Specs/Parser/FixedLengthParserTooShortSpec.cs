using System.Globalization;
using CsvHelper.Configuration;
using CsvHelper.FixedLengthParser;
using FluentAssertions;
using Xunit;

namespace CsvHelper.FixedLengthParser.Specs.Parser;

public class FixedLengthParserTooShortSpec
{
    [Fact]
    public void Throws_when_record_is_too_short()
    {
        // Arrange
        var tempPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".txt");
        // Missing one character for the age field ([5..7) requires length 7)
        var lines = new[] { "Bill 2" };
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

        // Act
        Action act = () =>
        {
            using var parser = new FixedLengthParser(tempPath, options, cfg);
            parser.Read();
        };

        // Assert
        act.Should().Throw<IndexOutOfRangeException>()
           .WithMessage("Record is too short.*");

        // Cleanup
        File.Delete(tempPath);
    }
}
