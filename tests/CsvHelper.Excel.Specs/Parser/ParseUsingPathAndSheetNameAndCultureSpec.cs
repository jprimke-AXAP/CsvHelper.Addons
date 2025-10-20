//-----------------------------------------------------------------------
// <copyright file="D:\PROJEKTE\AXAP.FileExchange\src\Utilities\CsvHelper.Excel\CsvHelper.Excel.Specs\Parser\ParseUsingPathAndSheetNameAndCultureSpec.cs" company="primsoft.NET">
// Author: Jörg H Primke
// Copyright (c) 2023 - primsoft.NET. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Globalization;

namespace CsvHelper.Excel.Specs.Parser;

public class ParseUsingPathAndSheetNameAndCultureSpec : ExcelParserSpec
{
    public ParseUsingPathAndSheetNameAndCultureSpec()
        : base("parse_by_path_and_sheetname_and_culture", "a_different_sheet_name")
    {
        using var parser = new ExcelParser(Path, WorksheetName, CultureInfo.InvariantCulture);
        Run(parser);
    }
}
