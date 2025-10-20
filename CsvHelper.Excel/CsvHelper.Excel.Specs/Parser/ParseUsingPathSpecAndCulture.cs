//-----------------------------------------------------------------------
// <copyright file="D:\PROJEKTE\AXAP.FileExchange\src\Utilities\CsvHelper.Excel\CsvHelper.Excel.Specs\Parser\ParseUsingPathSpecAndCulture.cs" company="primsoft.NET">
// Author: Jörg H Primke
// Copyright (c) 2023 - primsoft.NET. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Globalization;

namespace CsvHelper.Excel.Specs.Parser;

public class ParseUsingPathSpecAndCulture : ExcelParserSpec
{
    public ParseUsingPathSpecAndCulture()
        : base("parse_by_path_and_culture")
    {
        using var parser = new ExcelParser(Path, CultureInfo.InvariantCulture);
        Run(parser);
    }
}