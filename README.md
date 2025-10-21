# CsvHelper.Addons

[![Pack NuGet on version bump](https://github.com/jprimke-AXAP/CsvHelper.Addons/actions/workflows/nuget-pack.yml/badge.svg?branch=main)](https://github.com/jprimke-AXAP/CsvHelper.Addons/actions/workflows/nuget-pack.yml)
[![GitHub Release](https://img.shields.io/github/v/release/jprimke-AXAP/CsvHelper.Addons?display_name=tag)](https://github.com/jprimke-AXAP/CsvHelper.Addons/releases)

Language: English | [Deutsch](README.de.md)

Extensions for [CsvHelper](https://joshclose.github.io/CsvHelper/) to process Excel files and fixed-width text files.

This repository contains two independent libraries that integrate seamlessly with CsvHelper:

- `src/CsvHelper.Excel` – Read and write Excel files (XLSX) via CsvHelper
- `src/CsvHelper.FixedLengthParser` – Parser for fixed-width files

## Requirements

- .NET SDK (recommended: .NET 9)
- For Excel: ClosedXML (used as a NuGet dependency)

## Quick start

### Read Excel (XLSX)

```csharp
using System.Globalization;
using CsvHelper;
using CsvHelper.Excel;

using var parser = new ExcelParser("data.xlsx", sheetName: "Sheet1", culture: CultureInfo.InvariantCulture);
using var csv = new CsvReader(parser, CultureInfo.InvariantCulture);

// As dynamic records
foreach (var row in csv.GetRecords<dynamic>())
{
    // row.ColumnName
}
```

### Write Excel (XLSX)

```csharp
using System.Globalization;
using CsvHelper;
using CsvHelper.Excel;

var records = new[]
{
    new { FirstName = "Jane", LastName = "Doe" },
    new { FirstName = "John", LastName = "Smith" },
};

using var writer = new ExcelWriter("export.xlsx", sheetName: "export", culture: CultureInfo.InvariantCulture);
writer.WriteRecords(records);
```

### Parse fixed-width file

```csharp
using System;
using System.Collections.Generic;
using System.Globalization;
using CsvHelper;
using CsvHelper.FixedLengthParser;

// Index -> Range(Start, End) with end-exclusive semantics (like .NET Range)
var options = new FixedLengthOptions(new Dictionary<int, Range>
{
    { 0, new Range(0, 10) },  // Column 0: chars 0..9
    { 1, new Range(10, 20) }, // Column 1: chars 10..19
});

using var parser = new FixedLengthParser("data.txt", options, CultureInfo.InvariantCulture);
using var csv = new CsvReader(parser, CultureInfo.InvariantCulture);

foreach (var row in csv.GetRecords<dynamic>())
{
    // row.Field0, row.Field1, ...
}
```

Alternatively, you can annotate your data type with attributes. See the README in `CsvHelper.FixedLengthParser`.

## Build & test

- Build: `dotnet build`
- Tests: `dotnet test`

Tip: You can build/test projects individually, e.g. `dotnet build src/CsvHelper.Excel/CsvHelper.Excel.csproj`.

## Projects overview

- CsvHelper.Excel: Provides `ExcelParser` (IParser) and `ExcelWriter` (CsvWriter) to read/write Excel files via CsvHelper.
- CsvHelper.FixedLengthParser: Provides `FixedLengthParser` which splits fixed-width lines into columns based on ranges—configurable via options or attributes.

## Notes

- Excel support targets XLSX via ClosedXML. CSV files themselves are out of scope (CsvHelper handles CSV).
- `FixedLengthParser` uses end-exclusive ranges (e.g., `new Range(0, 10)` covers characters 0 to 9).
