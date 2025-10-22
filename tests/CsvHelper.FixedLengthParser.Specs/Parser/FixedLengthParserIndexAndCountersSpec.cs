using System.Globalization;
using CsvHelper.Configuration;
using CsvHelper.FixedLengthParser;
using FluentAssertions;
using Xunit;

namespace CsvHelper.FixedLengthParser.Specs.Parser;

public class FixedLengthParserIndexAndCountersSpec
{
    [Fact]
    public void Read_sets_record_and_counters_and_indexer()
    {
        // Arrange
        var tempPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".txt");
        var lines = new[] { "Bill 20", "Ben  30", "Weed 40" };
        File.WriteAllLines(tempPath, lines);

        var options = new FixedLengthOptions(new Dictionary<int, Range>
        {
            { 0, new Range(0, 5) },
            { 1, new Range(5, 7) },
        });

        var cfg = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            Delimiter = ";",
            TrimOptions = TrimOptions.Trim,
        };

        using (var parser = new FixedLengthParser(tempPath, options, cfg))
        {
            // Act + Assert line 1
            parser.Read().Should().BeTrue();
            parser.Row.Should().Be(1);
            parser.RawRow.Should().Be(1);
            parser.RawRecord.Should().Be(lines[0]);
            parser.Count.Should().Be(2);
            parser.Delimiter.Should().Be(";");
            parser[0].Should().Be("Bill");
            parser[1].Should().Be("20");
            parser[2].Should().BeNull(); // out of range

            // Act + Assert line 2
            parser.Read().Should().BeTrue();
            parser.Row.Should().Be(2);
            parser.RawRow.Should().Be(2);
            parser.RawRecord.Should().Be(lines[1]);

            // Act + Assert line 3
            parser.Read().Should().BeTrue();
            parser.Row.Should().Be(3);
            parser.RawRow.Should().Be(3);
            parser.RawRecord.Should().Be(lines[2]);

            // End of file
            parser.Read().Should().BeFalse();
        }

        // Cleanup
        File.Delete(tempPath);
    }
}
