//-----------------------------------------------------------------------
// <copyright file="D:\PROJEKTE\AXAP.FileExchange\src\Utilities\CsvHelper.Excel\CsvHelper.Excel.Specs\Writer\SerialiseUsingPathSpec.cs" company="primsoft.NET">
// Author: Jörg H Primke
// Copyright (c) 2023 - primsoft.NET. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Globalization;
using ClosedXML.Excel;
using CsvHelper.Excel.Specs.Common;
using Xunit.Abstractions;

namespace CsvHelper.Excel.Specs.Writer;

public class SerialiseUsingPathSpec : ExcelWriterSpec
{
    public SerialiseUsingPathSpec(ITestOutputHelper outputHelper)
        : base(outputHelper, "serialise_by_path")
    {
        using var excelWriter = new ExcelWriter(Path, CultureInfo.InvariantCulture);
        Run(excelWriter);
    }

    protected override XLWorkbook GetWorkbook()
    {
        return Helpers.GetOrCreateWorkbook(Path, WorksheetName);
    }

    protected override IXLWorksheet GetWorksheet()
    {
        return Helpers.GetOrCreateWorkbook(Path, WorksheetName).GetOrAddWorksheet(WorksheetName);
    }
}