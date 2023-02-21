// See https://aka.ms/new-console-template for more information

//Console.WriteLine("Hello, World!");

using OfficeOpenXml;
using OfficeOpenXml.Style;

ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

using var package = new ExcelPackage();
var worksheet = package.Workbook.Worksheets.Add("Test");
var cell = worksheet.Cells[2, 2];
cell.Value = "NN";
cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
cell.Style.Font.Bold = true;

package.SaveAsAsync("output.xlsx");