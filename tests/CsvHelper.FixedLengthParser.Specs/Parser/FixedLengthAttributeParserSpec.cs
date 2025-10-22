using CsvHelper.FixedLengthParser;
using FluentAssertions;
using Xunit;

namespace CsvHelper.FixedLengthParser.Specs.Parser;

public class FixedLengthAttributeParserSpec
{
    private class ModelWithUnsortedProps
    {
        [FixedLength(index: 1, start: 5, end: 7)]
        public string Age { get; init; } = string.Empty;

        [FixedLength(index: 0, start: 0, end: 5)]
        public string Name { get; init; } = string.Empty;
    }

    [Fact]
    public void Builds_options_from_attributes_with_correct_indices_and_ranges()
    {
        // Act
        var options = FixedLengthAttributeParser.GetFixedLengthOptions(typeof(ModelWithUnsortedProps));

        // Assert
        options.FieldLengths.Should().HaveCount(2);
        options.FieldLengths.Should().ContainKey(0);
        options.FieldLengths.Should().ContainKey(1);
        options[0].Should().Be(new Range(0, 5));
        options[1].Should().Be(new Range(5, 7));
    }
}
