using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.FixedLengthParser;
using FluentAssertions;
using Xunit;

namespace CsvHelper.FixedLengthParser.Specs.Parser;

public class FixedLengthParserWithAttributesSpec
{
    private class TestRecord
    {
        [FixedLength(index: 0, start: 0, end: 5)]
        public string Name { get; init; } = string.Empty;

        [FixedLength(index: 1, start: 5, end: 7)]
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
    public void Parses_records_using_path_and_attributes()
    {
        // Arrange
        var tempPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".txt");
        var lines = new[] { "Bill 20", "Ben  30", "Weed 40" };
        File.WriteAllLines(tempPath, lines);

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
            using var parser = new FixedLengthParser(tempPath, typeof(TestRecord), cfg);
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
