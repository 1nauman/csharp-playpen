// See https://aka.ms/new-console-template for more information

//Console.WriteLine("Hello, World!");

using OfficeOpenXml;
using OfficeOpenXml.Style;

ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

using var package = new ExcelPackage();
var worksheet = package.Workbook.Worksheets.Add("Test");
var cell = worksheet.Cells["B2"];
cell.Value = "NN";
cell.Style.Font.Bold = true;
cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

package.SaveAsAsync("output.xlsx");