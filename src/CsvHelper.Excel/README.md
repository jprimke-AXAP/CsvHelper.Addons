# CsvHelper.Excel

CsvHelper extension for reading and writing Excel files (XLSX) using [ClosedXML](https://github.com/ClosedXML/ClosedXML).

Language: English | [Deutsch](README.de.md)

Provided types:

- `ExcelParser` (implements `IParser`) for reading
- `ExcelWriter` (extends `CsvWriter`) for writing

## Installation

Add the NuGet package to your application (project dependency). ClosedXML is referenced transitively.

## Reading with ExcelParser

`ExcelParser` implements `IParser` and can be passed directly to `CsvReader`.

Overloads (excerpt):

- `ExcelParser(string path)`
- `ExcelParser(string path, string sheetName)`
- `ExcelParser(string path, CultureInfo culture)`
- `ExcelParser(string path, string sheetName, CultureInfo culture)`
- `ExcelParser(Stream stream, string? sheetName, CsvConfiguration configuration)`

Example:

```csharp
using System.Globalization;
using CsvHelper;
using CsvHelper.Excel;

using var parser = new ExcelParser("data.xlsx", sheetName: "Sheet1", culture: CultureInfo.InvariantCulture);
using var csv = new CsvReader(parser, CultureInfo.InvariantCulture);

// Map to classes using headers
var records = csv.GetRecords<Person>().ToList();
```

Notes:

- If `sheetName` is null/empty, the first worksheet is used.
- `TrimOptions` from `CsvConfiguration` are honored (e.g., `TrimOptions.Trim`).
- The number of columns (`Count`) is derived from the used cells of the worksheet.

## Writing with ExcelWriter

`ExcelWriter` extends `CsvWriter` and writes cell values into a new workbook/worksheet that is saved to the target stream on dispose.

Overloads (excerpt):

- `ExcelWriter(string path)` – writes to a workbook with worksheet name `export` (Invariant culture by default)
- `ExcelWriter(string path, string sheetName)`
- `ExcelWriter(string path, string sheetName, CultureInfo culture)`
- `ExcelWriter(Stream stream, string sheetName, CsvConfiguration configuration)`

Minimal example:

```csharp
using System.Globalization;
using CsvHelper.Excel;

var rows = new[]
{
    new { A = "1", B = "2" },
    new { A = "3", B = "4" },
};

using var writer = new ExcelWriter("export.xlsx", "export", CultureInfo.InvariantCulture);
writer.WriteRecords(rows);
```

Details:

- Injection protection: If `InjectionOptions.Escape` is set in the `CsvConfiguration`, fields are sanitized via `SanitizeForInjection`.
- `NextRecord()` resets the column position and increments the row number.
- On dispose, the workbook is saved to the stream and the stream is closed optionally (`leaveOpen`).

## Examples: mapping and options

```csharp
using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Excel;

var config = new CsvConfiguration(CultureInfo.InvariantCulture)
{
    HasHeaderRecord = true,
    TrimOptions = TrimOptions.Trim,
};

// Read
using var parser = new ExcelParser("input.xlsx", sheetName: "Data", culture: CultureInfo.InvariantCulture);
using var csv = new CsvReader(parser, config);
var records = csv.GetRecords<Person>().ToList();

// Write
using var writer = new ExcelWriter("output.xlsx", sheetName: "Export", culture: CultureInfo.InvariantCulture);
using var csvWriter = (CsvWriter)writer; // ExcelWriter is a CsvWriter
csvWriter.WriteRecords(records);
```

## Limitations and notes

- No formatting, styles, or formulas are set; raw cell values are written.
- For very large files: ClosedXML keeps the workbook in memory.
- `ExcelParser` reads row by row up to the last used row; empty cells are returned as empty strings.
