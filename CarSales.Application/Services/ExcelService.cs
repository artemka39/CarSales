using CarSales.Application.Interfaces;
using CarSales.Contracts.Dtos;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CarSales.Application.Services
{
    public class ExcelService : IExcelService
    {
        public byte[] CreateWorksheet(string title, Dictionary<string, List<string>> columns)
        {
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add(title);

            worksheet.Cell(1, 1).Value = title;
            worksheet.Range(1, 1, 1, columns.Count).Merge().Style
                .Font.SetBold()
                .Font.SetFontSize(14)
                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

            var col = 1;
            foreach (var column in columns)
            {
                worksheet.Cell(2, col).Value = column.Key;
                worksheet.Cell(2, col).Style.Font.SetBold();
                for (int row = 0; row < column.Value.Count; row++)
                {
                    worksheet.Cell(row + 3, col).Value = column.Value[row];
                }
                col++;
            }

            worksheet.Columns().AdjustToContents();

            using var ms = new MemoryStream();
            workbook.SaveAs(ms);
            return ms.ToArray();
        }
    }
}
