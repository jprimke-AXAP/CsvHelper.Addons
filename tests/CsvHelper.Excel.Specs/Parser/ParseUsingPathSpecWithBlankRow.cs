//-----------------------------------------------------------------------
// <copyright file="D:\PROJEKTE\AXAP.FileExchange\src\Utilities\CsvHelper.Excel\CsvHelper.Excel.Specs\Parser\ParseUsingPathSpecWithBlankRow.cs" company="primsoft.NET">
// Author: J�rg H Primke
// Copyright (c) 2023 - primsoft.NET. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Globalization;
using System.Linq;
using CsvHelper.Configuration;

namespace CsvHelper.Excel.Specs.Parser;

public class ParseUsingPathSpecWithBlankRow : ExcelParserSpec
{
    public ParseUsingPathSpecWithBlankRow()
        : base("parse_by_path_with_blank_row", includeBlankRow: true)
    {
        var csvConfiguration = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            ShouldSkipRecord = record => record.Row.Parser.Record.All(string.IsNullOrEmpty)
        };
        using var parser = new ExcelParser(Path, null, csvConfiguration);
        Run(parser);
    }
}