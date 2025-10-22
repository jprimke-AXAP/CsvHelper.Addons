using System.Globalization;
using CsvHelper.Configuration;
using CsvHelper.FixedLengthParser;
using FluentAssertions;
using Xunit;

namespace CsvHelper.FixedLengthParser.Specs.Parser;

public class FixedLengthParserDisposeSpec
{
    [Fact]
    public void Disposing_parser_disposes_underlying_stream()
    {
        // Arrange
        var tempPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".txt");
        File.WriteAllText(tempPath, "Bill 20\n");

        var options = new FixedLengthOptions(new Dictionary<int, Range>
        {
            { 0, new Range(0, 5) },
            { 1, new Range(5, 7) },
        });

        var cfg = new CsvConfiguration(CultureInfo.InvariantCulture);

        using var stream = File.OpenText(tempPath);
        var parser = new FixedLengthParser(stream, options, cfg);

        // Act
        parser.Dispose();

        // Assert
        Action act = () => stream.Peek();
        act.Should().Throw<ObjectDisposedException>();

        // Cleanup
        File.Delete(tempPath);
    }
}
