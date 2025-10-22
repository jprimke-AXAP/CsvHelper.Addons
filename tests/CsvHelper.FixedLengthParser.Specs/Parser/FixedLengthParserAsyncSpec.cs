using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.FixedLengthParser;
using FluentAssertions;
using Xunit;

namespace CsvHelper.FixedLengthParser.Specs.Parser;

public class FixedLengthParserAsyncSpec
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
    public async Task Parses_records_asynchronously()
    {
        // Arrange
        var tempPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".txt");
        var lines = new[] { "Bill 20", "Ben  30", "Weed 40" };
        await File.WriteAllLinesAsync(tempPath, lines);

        var options = new FixedLengthOptions(new Dictionary<int, Range>
        {
            { 0, new Range(0, 5) },
            { 1, new Range(5, 7) },
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
        List<TestRecord> results;
        using (var stream = File.OpenText(tempPath))
        using (var parser = new FixedLengthParser(stream, options, cfg))
        using (var reader = new CsvReader(parser))
        {
            reader.Context.RegisterClassMap(new TestMap());
            results = await reader.GetRecordsAsync<TestRecord>().ToListAsync();
        }

        // Assert
        results.Should().BeEquivalentTo(expected, o => o.WithStrictOrdering());

        // Cleanup
        File.Delete(tempPath);
    }
}
