//-----------------------------------------------------------------------
// <copyright file="D:\PROJEKTE\AXAP.FileExchange\src\Utilities\CsvHelper.Excel\CsvHelper.Excel.Specs\Parser\ParseUsingPathSpec.cs" company="primsoft.NET">
// Author: Jörg H Primke
// Copyright (c) 2023 - primsoft.NET. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace CsvHelper.Excel.Specs.Parser;

public class ParseUsingPathSpec : ExcelParserSpec
{
    public ParseUsingPathSpec()
        : base("parse_by_path")
    {
        using var parser = new ExcelParser(Path);
        Run(parser);
    }
}