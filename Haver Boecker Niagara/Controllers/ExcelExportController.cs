using Haver_Boecker_Niagara.Data;
using Haver_Boecker_Niagara.Models;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using OfficeOpenXml.Drawing.Chart;
using OfficeOpenXml.Style;
using System.IO;

namespace Haver_Boecker_Niagara.Controllers
{
    public class ExcelExportController : Controller
    {
        private readonly HaverContext _context;
        public ExcelExportController(HaverContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult ExportTimeLine(int? KickoffMeetingId)
        {
            if (KickoffMeetingId == null)
            {
                return NotFound();
            }
            var milestones = _context.Milestones
                .Where(g => g.KickOfMeetingID == KickoffMeetingId)
                .ToList() ;

            if (milestones.Count == 0)
            {
                return NotFound();   
            }

            ExcelPackage.License.SetNonCommercialPersonal("Gamaliel Romualdo");

            using (var package = new ExcelPackage())
            {
               var worksheet = package.Workbook.Worksheets.Add("Timeline");
                worksheet.Cells[1, 1].Value = "Milestone";
                worksheet.Cells[1, 2].Value = "Start Date";
                worksheet.Cells[1, 3].Value = "End Date";
                worksheet.Cells[1, 4].Value = "Task Status";

                var months = Enumerable.Range(1, 12)
                      .Select(m => new DateTime(2025, m, 1).ToString("MMM yyyy", System.Globalization.CultureInfo.InvariantCulture))
                      .ToList();


                worksheet.Cells[1, 5].Value = "Milestones Duration";
                for (int i = 0; i < milestones.Count; i++)
                {
                    var m = milestones[i];
                    worksheet.Cells[i + 2, 1].Value = m.Name;
                    worksheet.Cells[i + 2, 2].Value = m.StartDate;
                    worksheet.Cells[i + 2, 2].Style.Numberformat.Format = "dd mmm yyyy";
                    worksheet.Cells[i + 2, 3].Value = m.EndDate;
                    worksheet.Cells[i + 2, 3].Style.Numberformat.Format = "dd mmm yyyy";
                    worksheet.Cells[i + 2, 4].Value = m.Status;


                    if (m.StartDate.HasValue && m.EndDate.HasValue)
                    {
                        var duration = Math.Abs((m.EndDate.Value - m.StartDate.Value).TotalDays);
                        worksheet.Cells[i + 2, 5].Value = duration;
                    }

                }

                worksheet.Cells[1, 1, 1, 4].Style.Font.Bold = true;
                worksheet.Cells.AutoFitColumns();
                var sortedMilestones = milestones.OrderBy(g => g.StartDate).ToList();

                var chart = worksheet.Drawings.AddChart("TimelineChart", eChartType.ColumnClustered);
                chart.Title.Text = "Milestones Duration";

                var series = chart.Series.Add(
                    worksheet.Cells[2, 5, milestones.Count + 1, 5], 
                    worksheet.Cells[2, 1, milestones.Count + 1, 1]  
                );
                series.Header = "Duration of the milestone";

                chart.XAxis.Title.Text = "Milestone";
                chart.YAxis.Title.Text = "Days";
                chart.Legend.Remove();
                chart.SetPosition(1, 0, 6, 0);
                chart.SetSize(900, 400);



                var stream = new MemoryStream();
                package.SaveAs(stream);
                stream.Position = 0;   
                var fileName = $"GanttSchedule_{DateTime.Now:yyyyMMdd}.xlsx";
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
        }
    }
}
