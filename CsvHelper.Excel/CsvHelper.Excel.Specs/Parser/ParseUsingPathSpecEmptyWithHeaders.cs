//-----------------------------------------------------------------------
// <copyright file="D:\PROJEKTE\AXAP.FileExchange\src\Utilities\CsvHelper.Excel\CsvHelper.Excel.Specs\Parser\ParseUsingPathSpecEmptyWithHeaders.cs" company="primsoft.NET">
// Author: Jörg H Primke
// Copyright (c) 2023 - primsoft.NET. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace CsvHelper.Excel.Specs.Parser;

public class ParseUsingPathSpecEmptyWithHeaders : EmptySpecWithHeaders
{
    public ParseUsingPathSpecEmptyWithHeaders()
        : base("parse_by_path_empty_with_headers")
    {
        using var parser = new ExcelParser(Path);
        Run(parser);
    }
}