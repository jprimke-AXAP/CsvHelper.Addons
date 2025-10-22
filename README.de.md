# CsvHelper.Addons

[![Build and publish packages](https://github.com/jprimke-AXAP/CsvHelper.Addons/actions/workflows/publish-packages.yml/badge.svg?branch=main)](https://github.com/jprimke-AXAP/CsvHelper.Addons/actions/workflows/publish-packages.yml)
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

**Tipp:** Sie können Projekte auch einzeln bauen oder testen, z. B. mit `dotnet build src/CsvHelper.Excel/CsvHelper.Excel.csproj`.
## Projekte im Überblick

- CsvHelper.Excel: `ExcelParser` (IParser) und `ExcelWriter` (CsvWriter) zum Lesen/Schreiben von Excel-Dateien.
- CsvHelper.FixedLengthParser: `FixedLengthParser` teilt Festlängen-Zeilen in Spalten gemäß Ranges auf – konfigurierbar über Optionen oder Attribute.

## Hinweise

- XLSX-Unterstützung über ClosedXML. CSV-Dateien selbst sind hier nicht enthalten (dafür ist CsvHelper zuständig).
- `FixedLengthParser` nutzt end-exklusive Ranges (z. B. `new Range(0, 10)` umfasst Zeichen 0 bis 9).

## CI: Paketierung und Releases

- Bei Pushes auf `main`, die Projektdateien (`.csproj`), Lösungsdateien oder Code unter `src/` betreffen, führt GitHub Actions Build, Tests und das Packen nur der geänderten Projekte aus.
- Erstellte Pakete werden:
    - in GitHub Packages veröffentlicht (Owner-Feed unter <https://github.com/orgs/${owner}/packages>)
    - einem GitHub Release mit dem Tag `packages-<run>-<sha>` beigefügt.

Das Verwenden aus GitHub Packages erfordert das Hinzufügen einer NuGet-Quelle:

```sh
dotnet nuget add source "https://nuget.pkg.github.com/<owner>/index.json" --name github \
        --username <owner> --password <PAT with read:packages> --store-password-in-clear-text
```

Es sind keine zusätzlichen Secrets für das Veröffentlichen nötig; der Workflow nutzt das integrierte `GITHUB_TOKEN` des Repos.

## Optional: Version bei jedem Build automatisch erhöhen

Dieses Repo nutzt Nerdbank.GitVersioning (NB.GV) für die Basis-Semantikversionierung (siehe `version.json`). Sie können eine pro-Build eindeutige Version aktivieren, ohne Git-Tags zu ändern, indem Sie eine MSBuild-Eigenschaft setzen:

- Bei Aktivierung wird ein UTC-Zeitstempel an die Basisversion angehängt, z. B. `1.8.0-ci.20251021.122744`.
- Der Suffix wird auf die NuGet-`PackageVersion` und `AssemblyInformationalVersion` angewendet. Die Assembly-Dateiversion kann je nach NB.GV-Einstellungen stabil bleiben.

Aktivierung:

```powershell
dotnet build CsvHelper.Addons.slnx -c Release -p:AutoBuildVersion=true
dotnet pack  CsvHelper.Addons.slnx -c Release -p:AutoBuildVersion=true -o .\nupkgs
```

Hinweise:

- Für normale Release-Versionen `AutoBuildVersion=true` weglassen (NB.GV vergibt die Versionen).
- CI: die Umgebungsvariable `AutoBuildVersion=true` setzen oder als MSBuild-Property übergeben.
- Zeitstempelformat: `yyyyMMdd.HHmmss` (UTC) für monotone, sortierbare Versionen.
