# CsvHelper.Addons

[![Pack NuGet on version bump](https://github.com/jprimke-AXAP/CsvHelper.Addons/actions/workflows/nuget-pack.yml/badge.svg?branch=main)](https://github.com/jprimke-AXAP/CsvHelper.Addons/actions/workflows/nuget-pack.yml)
[![GitHub Release](https://img.shields.io/github/v/release/jprimke-AXAP/CsvHelper.Addons?display_name=tag)](https://github.com/jprimke-AXAP/CsvHelper.Addons/releases)

Sprache: Deutsch | [English](README.md)

Erweiterungen für [CsvHelper](https://joshclose.github.io/CsvHelper/) zur Verarbeitung von Excel-Dateien und Festlängen-Dateien.

Dieses Repository enthält zwei eigenständige Bibliotheken, die sich nahtlos in CsvHelper integrieren:

- `src/CsvHelper.Excel` – Lesen und Schreiben von Excel-Dateien (XLSX) über CsvHelper
- `src/CsvHelper.FixedLengthParser` – Parser für Festlängen-Dateien

## Voraussetzungen

- .NET SDK (empfohlen: .NET 9)
- Für Excel: ClosedXML (als NuGet-Abhängigkeit)

## Schnellstart

### Excel lesen (XLSX)

```csharp
using System.Globalization;
using CsvHelper;
using CsvHelper.Excel;

using var parser = new ExcelParser("data.xlsx", sheetName: "Sheet1", culture: CultureInfo.InvariantCulture);
using var csv = new CsvReader(parser, CultureInfo.InvariantCulture);

foreach (var row in csv.GetRecords<dynamic>())
{
    // row.Spaltenname
}
```

### Excel schreiben (XLSX)

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

### Festlängen-Datei parsen

```csharp
using System;
using System.Collections.Generic;
using System.Globalization;
using CsvHelper;
using CsvHelper.FixedLengthParser;

// Index -> Range(Start, End) mit end-exklusiv (wie .NET Range)
var options = new FixedLengthOptions(new Dictionary<int, Range>
{
    { 0, new Range(0, 10) },  // Spalte 0: Zeichen 0..9
    { 1, new Range(10, 20) }, // Spalte 1: Zeichen 10..19
});

using var parser = new FixedLengthParser("data.txt", options, CultureInfo.InvariantCulture);
using var csv = new CsvReader(parser, CultureInfo.InvariantCulture);

foreach (var row in csv.GetRecords<dynamic>())
{
    // row.Field0, row.Field1, ...
}
```

Alternativ können Sie Attribute an Ihrem Datentyp verwenden. Siehe `src/CsvHelper.FixedLengthParser`.

## Build & Tests

- Build: `dotnet build`
- Tests: `dotnet test`

## Projekte im Überblick

- CsvHelper.Excel: `ExcelParser` (IParser) und `ExcelWriter` (CsvWriter) zum Lesen/Schreiben von Excel-Dateien.
- CsvHelper.FixedLengthParser: `FixedLengthParser` teilt Festlängen-Zeilen in Spalten gemäß Ranges auf – konfigurierbar über Optionen oder Attribute.

## Hinweise

- XLSX-Unterstützung über ClosedXML. CSV-Dateien selbst sind hier nicht enthalten (dafür ist CsvHelper zuständig).
- `FixedLengthParser` nutzt end-exklusive Ranges (z. B. `new Range(0, 10)` umfasst Zeichen 0 bis 9).
