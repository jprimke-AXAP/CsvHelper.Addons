//-----------------------------------------------------------------------
// <copyright file="D:\PROJEKTE\GenericData\src\Common\CsvHelper.FixedLengthParser\FixedLengthParser.cs" company="primsoft.NET">
// Author: Jörg H Primke
// Copyright (c) 2023 - primsoft.NET. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Globalization;
using System.Runtime.CompilerServices;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;

namespace CsvHelper.FixedLengthParser;

#pragma warning disable S112

public class FixedLengthParser : IParser
{
    private readonly FixedLengthOptions fixedLengthOptions;
    private readonly StreamReader stream;

    private bool disposed;

    public FixedLengthParser(string path, FixedLengthOptions fixedLengthOptions)
        : this(path, fixedLengthOptions, CultureInfo.InvariantCulture)
    {
    }

    public FixedLengthParser(string path, FixedLengthOptions fixedLengthOptions, CultureInfo cultureInfo)
        : this(path, fixedLengthOptions, new CsvConfiguration(cultureInfo))
    {
    }

    public FixedLengthParser(string path, FixedLengthOptions fixedLengthOptions, CsvConfiguration csvConfiguration)
        : this(File.OpenText(path), fixedLengthOptions, csvConfiguration)
    {
    }

    public FixedLengthParser(string path, Type type)
        : this(path, type, CultureInfo.InvariantCulture)
    {
    }

    public FixedLengthParser(string path, Type type, CultureInfo cultureInfo)
        : this(path, type, new CsvConfiguration(cultureInfo))
    {
    }

    public FixedLengthParser(string path, Type type, CsvConfiguration csvConfiguration)
        : this(File.OpenText(path), type, csvConfiguration)
    {
    }

    public FixedLengthParser(StreamReader stream, Type type)
        : this(stream, FixedLengthAttributeParser.GetFixedLengthOptions(type), CultureInfo.InvariantCulture)
    {
    }

    public FixedLengthParser(StreamReader stream, Type type, CultureInfo cultureInfo)
        : this(stream, FixedLengthAttributeParser.GetFixedLengthOptions(type), new CsvConfiguration(cultureInfo))
    {
    }

    public FixedLengthParser(StreamReader stream, Type type, CsvConfiguration csvConfiguration)
        : this(stream, FixedLengthAttributeParser.GetFixedLengthOptions(type), csvConfiguration)
    {
    }

    public FixedLengthParser(StreamReader stream, FixedLengthOptions fixedLengthOptions)
        : this(stream, fixedLengthOptions, CultureInfo.InvariantCulture)
    {
    }

    public FixedLengthParser(StreamReader stream, FixedLengthOptions fixedLengthOptions, CultureInfo cultureInfo)
        : this(stream, fixedLengthOptions, new CsvConfiguration(cultureInfo))
    {
    }

    public FixedLengthParser(StreamReader stream, FixedLengthOptions fixedLengthOptions, CsvConfiguration csvConfiguration)
    {
        this.stream = stream;
        this.fixedLengthOptions = fixedLengthOptions;
        Configuration = csvConfiguration;
        Context = new CsvContext(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposed)
        {
            return;
        }

        disposed = true;

        if (!disposing)
        {
            return;
        }

        stream.Dispose();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private bool HandleRecord(string[]? record)
    {
        if (record is null)
        {
            return false;
        }

        Record = record;
        Row++;
        RawRow++;

        return true;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private string[]? GetRecord()
    {
        var currentLine = stream.ReadLine();
        return SplitLine(currentLine);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private async Task<string[]?> GetRecordAsync()
    {
        var currentLine = await stream.ReadLineAsync().ConfigureAwait(false);
        return SplitLine(currentLine);
    }

    private string[]? SplitLine(string? currentLine)
    {
        if (currentLine is null)
        {
            return null;
        }

        RawRecord = currentLine;
        var currentRecord = new string[fixedLengthOptions.FieldLengths.Count];

        var recordAsSpan = currentLine.AsSpan();
        foreach (var (index, range) in fixedLengthOptions.FieldLengths)
        {
            var length = range.End.Value - range.Start.Value;
            if (recordAsSpan.Length < range.End.Value)
            {
                throw new IndexOutOfRangeException($"Record is too short. Expected {range.End.Value} but was {recordAsSpan.Length}.");
            }

            var field = recordAsSpan.Slice(range.Start.Value, length).ToString();
            if (Configuration.TrimOptions.HasFlag(TrimOptions.Trim))
            {
                field = field.Trim();
            }

            currentRecord[index] = field;
        }

        return currentRecord;
    }

    #region IParser properties

    public string? this[int index] => Record?.ElementAtOrDefault(index);

    public long ByteCount => -1;

    public long CharCount => -1;

    public int Count => fixedLengthOptions.FieldLengths.Count;

    public string[]? Record { get; private set; }

    public string? RawRecord { get; private set; }

    public int Row { get; private set; }

    public int RawRow { get; private set; }

    public string Delimiter => Configuration.Delimiter;

    public CsvContext Context { get; }

    public IParserConfiguration Configuration { get; }

    #endregion

    #region IParser implementation

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public bool Read()
    {
        var record = GetRecord();
        return HandleRecord(record);
    }

    public async Task<bool> ReadAsync()
    {
        var record = await GetRecordAsync().ConfigureAwait(false);
        return HandleRecord(record);
    }

    #endregion
}

public record FixedLengthOptions(Dictionary<int, Range> FieldLengths)
{
    public Range this[int index]
    {
        set => FieldLengths[index] = value;
        get => FieldLengths.TryGetValue(index, out var value) ? value : throw new IndexOutOfRangeException();
    }
}

[AttributeUsage(AttributeTargets.Property)]
public class FixedLengthAttribute(int index, int start, int end) : IndexAttribute(index)
{
    public int Start { get; init; } = start;

    public int End { get; init; } = end;
}

public static class FixedLengthAttributeParser
{
    public static FixedLengthOptions GetFixedLengthOptions(Type type)
    {
        var ranges = type.GetProperties().Select(item => item.GetCustomAttributes(true).OfType<FixedLengthAttribute>().FirstOrDefault()).OfType<FixedLengthAttribute>().ToDictionary(fixedLengthAttribute => fixedLengthAttribute.Index, fixedLengthAttribute => new Range(fixedLengthAttribute.Start, fixedLengthAttribute.End));

        return new FixedLengthOptions(ranges);
    }
}

#pragma warning restore S112 // General or reserved exceptions should never be thrown
