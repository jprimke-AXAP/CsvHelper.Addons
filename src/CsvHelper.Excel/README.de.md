# CsvHelper.Excel

Erweiterung für CsvHelper zum Lesen und Schreiben von Excel-Dateien (XLSX) mit [ClosedXML](https://github.com/ClosedXML/ClosedXML).

Sprache: Deutsch | [English](README.md)

Bereitgestellte Typen:

- `ExcelParser` (implementiert `IParser`) zum Lesen
- `ExcelWriter` (erweitert `CsvWriter`) zum Schreiben

## Installation

Fügen Sie das NuGet-Paket Ihrem Projekt hinzu. ClosedXML wird transitiv referenziert.

## Lesen mit ExcelParser

`ExcelParser` implementiert `IParser` und kann direkt an `CsvReader` übergeben werden.

Überladungen (Auswahl):

- `ExcelParser(string path)`
- `ExcelParser(string path, string sheetName)`
- `ExcelParser(string path, CultureInfo culture)`
- `ExcelParser(string path, string sheetName, CultureInfo culture)`
- `ExcelParser(Stream stream, string? sheetName, CsvConfiguration configuration)`

Beispiel:

```csharp
using System.Globalization;
using CsvHelper;
using CsvHelper.Excel;

using var parser = new ExcelParser("data.xlsx", sheetName: "Sheet1", culture: CultureInfo.InvariantCulture);
using var csv = new CsvReader(parser, CultureInfo.InvariantCulture);

var records = csv.GetRecords<Person>().ToList();
```

Hinweise:

- Ist `sheetName` null/leer, wird das erste Arbeitsblatt verwendet.
- `TrimOptions` der `CsvConfiguration` werden berücksichtigt (z. B. `TrimOptions.Trim`).
- Die Spaltenanzahl (`Count`) wird aus den verwendeten Zellen ermittelt.

## Schreiben mit ExcelWriter

`ExcelWriter` erweitert `CsvWriter` und schreibt Werte in ein neues Workbook/Worksheet, das beim Dispose in den Zielstream gespeichert wird.

Überladungen (Auswahl):

- `ExcelWriter(string path)` – schreibt in das Worksheet `export` (Standard: Invariant Culture)
- `ExcelWriter(string path, string sheetName)`
- `ExcelWriter(string path, string sheetName, CultureInfo culture)`
- `ExcelWriter(Stream stream, string sheetName, CsvConfiguration configuration)`

Minimalbeispiel:

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

- Injection-Schutz: Wenn `InjectionOptions.Escape` gesetzt ist, werden Felder per `SanitizeForInjection` gesichert.
- `NextRecord()` setzt die Spaltenposition zurück und erhöht die Zeilennummer.
- Beim Dispose wird das Workbook gespeichert und der Stream optional geschlossen (`leaveOpen`).

## Beispiele: Mapping und Optionen

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

// Lesen
using var parser = new ExcelParser("input.xlsx", sheetName: "Daten", culture: CultureInfo.InvariantCulture);
using var csv = new CsvReader(parser, config);
var records = csv.GetRecords<Person>().ToList();

// Schreiben
using var writer = new ExcelWriter("output.xlsx", sheetName: "Export", culture: CultureInfo.InvariantCulture);
using var csvWriter = (CsvWriter)writer; // ExcelWriter ist CsvWriter
csvWriter.WriteRecords(records);
```

## Grenzen und Hinweise

- Keine Formatierung/Styles/Formeln – es werden reine Zellwerte geschrieben.
- Bei sehr großen Dateien beachten: ClosedXML hält das Workbook im Speicher.
- `ExcelParser` liest bis zur letzten verwendeten Zeile; leere Zellen werden als leere Strings geliefert.
