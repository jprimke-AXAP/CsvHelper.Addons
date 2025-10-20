//-----------------------------------------------------------------------
// <copyright file="D:\PROJEKTE\AXAP.FileExchange\src\Utilities\CsvHelper.Excel\CsvHelper.Excel.Specs\Parser\ParseUsingStreamSpec.cs" company="primsoft.NET">
// Author: Jörg H Primke
// Copyright (c) 2023 - primsoft.NET. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Globalization;
using System.IO;

namespace CsvHelper.Excel.Specs.Parser;

public class ParseUsingStreamSpec : ExcelParserSpec
{
    public ParseUsingStreamSpec()
        : base("parse_by_stream")
    {
        using var stream = File.OpenRead(Path);
        using var parser = new ExcelParser(stream, CultureInfo.InvariantCulture);
        Run(parser);
    }
}