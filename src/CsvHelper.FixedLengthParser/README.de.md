# CsvHelper.FixedLengthParser

Parser-Erweiterung für CsvHelper zur Verarbeitung von Festlängen-Dateien. Jede Zeile wird anhand vordefinierter Zeichenbereiche in Felder zerschnitten.

Sprache: Deutsch | [English](README.md)

Bereitgestellte Typen:

- `FixedLengthParser` – implementiert `IParser`
- `FixedLengthOptions` – konfiguriert Feldbereiche per Index
- `FixedLengthAttribute` – Attribut zur Konfiguration direkt am Modelltyp
- `FixedLengthAttributeParser` – Hilfsklasse zum Erzeugen der Optionen aus Attributen

## Grundprinzip

- Ein Datensatz entspricht einer Textzeile.
- Felder werden mit 0-basierten Ranges definiert: `new Range(start, end)` mit end-exklusiver Semantik (wie .NET `Range`).
- Beispiel: `new Range(0, 10)` umfasst die Zeichen 0..9 (10 Zeichen).

## Verwendung mit FixedLengthOptions

```csharp
using System;
using System.Collections.Generic;
using System.Globalization;
using CsvHelper;
using CsvHelper.FixedLengthParser;

var options = new FixedLengthOptions(new Dictionary<int, Range>
{
    { 0, new Range(0, 10) },  // Spalte 0
    { 1, new Range(10, 20) }, // Spalte 1
});

using var parser = new FixedLengthParser("data.txt", options, CultureInfo.InvariantCulture);
using var csv = new CsvReader(parser, CultureInfo.InvariantCulture);

foreach (var rec in csv.GetRecords<dynamic>())
{
    // rec.Field0, rec.Field1, ...
}
```

## Verwendung mit Attributen

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

## API-Überblick

Konstruktoren (Auswahl):

- `FixedLengthParser(string path, FixedLengthOptions options)` (+ Overloads mit Kultur/Config)
- `FixedLengthParser(StreamReader stream, FixedLengthOptions options)` (+ Overloads)
- `FixedLengthParser(string path, Type type)` (+ Overloads) – erzeugt Optionen aus Attributen des Typs

Eigenschaften (IParser):

- `int Count` – Anzahl Spalten
- `string[]? Record` – aktueller Datensatz
- `string? RawRecord` – aktuelle Rohzeile
- `int Row` / `int RawRow` – Datensatz-/Rohzeilennummer
- `TrimOptions` aus `CsvConfiguration` werden berücksichtigt

## Fehlerbehandlung und Kantenfälle

- Wenn eine Zeile kürzer als ein definierter Bereich ist, wird `IndexOutOfRangeException` ausgelöst.
- Leere Zeilen liefern einen null-Record und beenden den Stream.
- Trimmen: Wenn `TrimOptions.Trim` gesetzt ist, werden Felder vor der Rückgabe getrimmt.

## Tipps

- Bereiche so definieren, dass alle erwarteten Spalten abgedeckt sind und sich nicht überlappen.
- Attribute am Modell verwenden, um Definitionen zentral und typsicher zu halten.
