using DynamicHighCharts.Entity.POCO;
using Newtonsoft.Json;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DynamicHighCharts.Controllers
{
    public class HomeController : Controller
    {
        private readonly CodeHubsContext _context = null;
        public HomeController()
        {
            _context = new CodeHubsContext();
        }
        public ActionResult Index() => View();

        public void AddExcelData()
        {
            try
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                List<Analysis> excelData = new List<Analysis>();
                FileInfo existingFile = new FileInfo($"{AppDomain.CurrentDomain.BaseDirectory}/Content/SampleDBData.xlsx");
                using (ExcelPackage package = new ExcelPackage(existingFile))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                    int rowCount = worksheet.Dimension.End.Row;
                    for (int row = 1; row <= rowCount; row++)
                    {
                        try
                        {
                            excelData.Add(new Analysis()
                            {
                                Description = worksheet.Cells[row, 1].Value.ToString().Trim(),
                                Date = worksheet.Cells[row, 2].Value.ToString().Trim(),
                                Value = worksheet.Cells[row, 3].Value.ToString().Trim()
                            });
                        }
                        catch (Exception)
                        {

                        }
                    }
                    _context.Analyses.AddRange(excelData);
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
            }
        }

        public JsonResult GetHighChartsData()
        {
            try
            {
                var highChartsData = _context.Analyses.AsEnumerable().OrderBy(x => Convert.ToDateTime(x.Date)).GroupBy(x => new { x.Description, x.Date }).Select(x => new
                {
                    Title = x.Key.Description,
                    Date = x.Key.Date,
                    Value = x.Sum(y => decimal.Parse(y.Value, CultureInfo.InvariantCulture))
                }).ToList();
                return Json(new { isSuccess = true, data = JsonConvert.SerializeObject(highChartsData) }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { isSuccess = false, message = ex.Message });
            }
        }
    }
}