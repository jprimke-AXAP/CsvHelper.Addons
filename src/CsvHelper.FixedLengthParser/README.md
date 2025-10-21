# CsvHelper.FixedLengthParser

CsvHelper parser extension for processing fixed-width files. Each line is sliced into fields based on predefined character ranges.

Language: English | [Deutsch](README.de.md)

Provided types:

- `FixedLengthParser` – implements `IParser`
- `FixedLengthOptions` – configures field ranges by index
- `FixedLengthAttribute` – attribute to configure ranges directly on the model type
- `FixedLengthAttributeParser` – helper to build options from attributes

## Basics

- A record corresponds to one text line.
- Fields are defined with 0-based ranges: `new Range(start, end)` with end-exclusive semantics (like .NET `Range`).
- Example: `new Range(0, 10)` covers characters 0..9 (10 chars).

## Using FixedLengthOptions

```csharp
using System;
using System.Collections.Generic;
using System.Globalization;
using CsvHelper;
using CsvHelper.FixedLengthParser;

var options = new FixedLengthOptions(new Dictionary<int, Range>
{
    { 0, new Range(0, 10) },  // Column 0
    { 1, new Range(10, 20) }, // Column 1
});

using var parser = new FixedLengthParser("data.txt", options, CultureInfo.InvariantCulture);
using var csv = new CsvReader(parser, CultureInfo.InvariantCulture);

foreach (var rec in csv.GetRecords<dynamic>())
{
    // rec.Field0, rec.Field1, ...
}
```

## Using attributes

```csharp
using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.FixedLengthParser;

public class Person
{
    [FixedLength(index: 0, start: 0, end: 10)]
    public string FirstName { get; set; } = string.Empty;

    [FixedLength(index: 1, start: 10, end: 30)]
    public string LastName { get; set; } = string.Empty;
}

var config = new CsvConfiguration(CultureInfo.InvariantCulture)
{
    TrimOptions = TrimOptions.Trim,
};

using var parser = new FixedLengthParser("people.txt", typeof(Person), config);
using var csv = new CsvReader(parser, config);
var people = csv.GetRecords<Person>().ToList();
```

## API overview

Constructors (selection):

- `FixedLengthParser(string path, FixedLengthOptions options)` (+ overloads with culture/config)
- `FixedLengthParser(StreamReader stream, FixedLengthOptions options)` (+ overloads)
- `FixedLengthParser(string path, Type type)` (+ overloads) – builds options from the type's attributes

Properties (IParser):

- `int Count` – number of fields
- `string[]? Record` – current record
- `string? RawRecord` – current raw line
- `int Row` / `int RawRow` – record/raw line number
- `TrimOptions` from `CsvConfiguration` are respected

## Error handling and edge cases

- If a line is shorter than a defined range, `IndexOutOfRangeException` is thrown.
- Empty lines result in a null record and end the stream.
- Trimming: If `TrimOptions.Trim` is set, fields are trimmed before being returned.

## Tips

- Define ranges to cover all expected columns without overlap.
- Prefer attributes on models to keep definitions centralized and type-safe.
