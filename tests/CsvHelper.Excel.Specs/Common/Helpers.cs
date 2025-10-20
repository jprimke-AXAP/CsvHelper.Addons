//-----------------------------------------------------------------------
// <copyright file="D:\PROJEKTE\AXAP.FileExchange\src\Utilities\CsvHelper.Excel\CsvHelper.Excel.Specs\Common\Helpers.cs" company="primsoft.NET">
// Author: Jörg H Primke
// Copyright (c) 2023 - primsoft.NET. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.IO;
using ClosedXML.Excel;

namespace CsvHelper.Excel.Specs.Common;

public static class Helpers
{
    public static XLWorkbook GetOrCreateWorkbook(string path, string worksheetName)
    {
        if (!File.Exists(path))
        {
            var workbook = new XLWorkbook();
            workbook.GetOrAddWorksheet(worksheetName);
            workbook.SaveAs(path);
            return workbook;
        }

        return new XLWorkbook(path);
    }

    public static IXLWorksheet GetOrAddWorksheet(this XLWorkbook workbook, string sheetName)
    {
        if (!workbook.TryGetWorksheet(sheetName, out var worksheet))
        {
            worksheet = workbook.AddWorksheet(sheetName);
        }

        return worksheet;
    }

    public static void Delete(string path)
    {
        var directory = Path.GetDirectoryName(path);
        Directory.Delete(directory!, true);
    }
}
