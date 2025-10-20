//-----------------------------------------------------------------------
// <copyright file="D:\PROJEKTE\AXAP.FileExchange\src\Utilities\CsvHelper.Excel\CsvHelper.Excel.Specs\Parser\ParseUsingPathAndSheetNameSpec.cs" company="primsoft.NET">
// Author: Jörg H Primke
// Copyright (c) 2023 - primsoft.NET. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace CsvHelper.Excel.Specs.Parser;

public class ParseUsingPathAndSheetNameSpec : ExcelParserSpec
{
    public ParseUsingPathAndSheetNameSpec()
        : base("parse_by_path_and_sheetname", "a_different_sheet_name")
    {
        using var parser = new ExcelParser(Path, WorksheetName);
        Run(parser);
    }
}