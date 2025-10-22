using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.FixedLengthParser;
using FluentAssertions;
using Xunit;

namespace CsvHelper.FixedLengthParser.Specs.Parser;

public class FixedLengthParserWithOptionsSpec
{
    private record TestRecord
    {
        public string Name { get; init; } = string.Empty;
        public int Age { get; init; }
    }

    private sealed class TestMap : ClassMap<TestRecord>
    {
        public TestMap()
        {
            Map(m => m.Name).Index(0);
            Map(m => m.Age).Index(1);
        }
    }

    [Fact]
    public void Parses_records_using_path_and_options()
    {
        // Arrange
        var tempPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".txt");
        var lines = new[] {
            "Bill 20",
            "Ben  30",
            "Weed 40",
        };
        File.WriteAllLines(tempPath, lines);

        var options = new FixedLengthOptions(new Dictionary<int, Range>
        {
            { 0, new Range(0, 5) }, // Name: columns [0..5)
            { 1, new Range(5, 7) }, // Age:  columns [5..7)
        });

        var cfg = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            TrimOptions = TrimOptions.Trim,
            HasHeaderRecord = false,
            HeaderValidated = null,
            MissingFieldFound = null,
        };

        var expected = new[]
        {
            new TestRecord { Name = "Bill", Age = 20 },
            new TestRecord { Name = "Ben",  Age = 30 },
            new TestRecord { Name = "Weed", Age = 40 },
        };

        // Act
        TestRecord[] results;
        {
            using var parser = new FixedLengthParser(tempPath, options, cfg);
            using var reader = new CsvReader(parser);
            reader.Context.RegisterClassMap(new TestMap());
            results = reader.GetRecords<TestRecord>().ToArray();
        }

        // Assert
        results.Should().BeEquivalentTo(expected, o => o.WithStrictOrdering());

        // Cleanup
        File.Delete(tempPath);
    }
}
