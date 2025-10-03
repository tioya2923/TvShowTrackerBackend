using Microsoft.AspNetCore.Mvc;
using TvShowTracker.Services;

namespace TvShowTracker.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExportController : ControllerBase
    {
        private readonly ExportService _exportService;

        public ExportController(ExportService exportService)
        {
            _exportService = exportService;
        }

        [HttpGet("{userId}/csv")]
        public IActionResult ExportCsv(int userId)
        {
            var csv = _exportService.ExportUserDataAsCsv(userId);
            var bytes = System.Text.Encoding.UTF8.GetBytes(csv);
            return File(bytes, "text/csv", $"user_{userId}_data.csv");
        }

        [HttpGet("{userId}/pdf")]
        public IActionResult ExportPdf(int userId)
        {
            var pdfBytes = _exportService.ExportUserDataAsPdf(userId);
            return File(pdfBytes, "application/pdf", $"user_{userId}_data.pdf");
          

        }
    }
}
