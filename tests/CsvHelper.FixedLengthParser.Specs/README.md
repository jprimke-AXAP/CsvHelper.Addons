# CsvHelper.FixedLengthParser.Specs

xUnit test suite for the `CsvHelper.FixedLengthParser` library. These tests validate parsing behavior for fixed-length records, error handling, trimming, attribute-based configuration, async flows, and resource management.

## What’s covered

- Parsing with explicit `FixedLengthOptions` and via `[FixedLength]` attributes
- Mapping fields by index using `ClassMap`
- Headerless inputs (CsvHelper configured with `HasHeaderRecord = false`)
- Trimming behavior (`TrimOptions.None` vs `TrimOptions.Trim`)
- Parser counters and members (`Row`, `RawRow`, `RawRecord`, `Count`, indexer)
- Error handling (too short records, blank lines)
- Async read path (`GetRecordsAsync<T>()`)
- Proper disposal of the underlying `StreamReader`

## Layout

- `Parser/`
  - `FixedLengthParserWithOptionsSpec.cs` – Happy path using `FixedLengthOptions`
  - `FixedLengthParserWithAttributesSpec.cs` – Happy path using `[FixedLength]` attributes
  - `FixedLengthParserAsyncSpec.cs` – Async read flow
  - `FixedLengthParserIndexAndCountersSpec.cs` – Indexer, counters, delimiter
  - `FixedLengthParserTrimBehaviorSpec.cs` – Trimming modes
  - `FixedLengthParserTooShortSpec.cs` – Too-short record throws
  - `FixedLengthParserBlankLineSpec.cs` – Blank line throws
  - `FixedLengthParserDisposeSpec.cs` – Disposing parser disposes stream
  - `FixedLengthAttributeParserSpec.cs` – Attribute parser builds correct ranges
  - `FixedLengthOptionsIndexerSpec.cs` – Options indexer get/set/throw

## Conventions

- Fixed-length files used in tests are headerless. We configure `CsvConfiguration` accordingly:
  - `HasHeaderRecord = false`
  - `HeaderValidated = null`
  - `MissingFieldFound = null`
- Field mapping uses a `ClassMap` with `.Index(n)` to bind columns to properties.
- Ranges are treated as half-open `[start, end)`; the parser throws if the line is shorter than the largest `end` value.
- Temporary files are created under the system temp folder and deleted after the parser/reader are disposed.

## How to run

From the repository root:

```powershell
# Run the entire solution tests
dotnet test

# Or just this project
dotnet test .\tests\CsvHelper.FixedLengthParser.Specs\CsvHelper.FixedLengthParser.Specs.csproj
```

## Notes

- Packages are centrally managed via `Directory.Packages.props`.
- Target framework and common settings come from `Directory.Build.props` (net9.0).
- If you see NuGet source mapping warnings, they are non-blocking for local tests.