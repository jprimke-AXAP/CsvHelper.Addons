using CsvHelper.FixedLengthParser;
using FluentAssertions;
using Xunit;

namespace CsvHelper.FixedLengthParser.Specs.Parser;

public class FixedLengthOptionsIndexerSpec
{
    [Fact]
    public void Indexer_get_and_set_work()
    {
        // Arrange
        var options = new FixedLengthOptions(new Dictionary<int, Range>
        {
            { 0, new Range(0, 5) },
        });

        // Act
        options[1] = new Range(5, 10);

        // Assert
        options[0].Should().Be(new Range(0, 5));
        options[1].Should().Be(new Range(5, 10));

        // Out of range get should throw
        Action act = () => { var _ = options[2]; };
        act.Should().Throw<IndexOutOfRangeException>();
    }
}
