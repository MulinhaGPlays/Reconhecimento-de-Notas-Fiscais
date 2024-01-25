using Microsoft.AspNetCore.Mvc;
using PrototipoLeitorTesseract.Models;
using System.Diagnostics;
using System.Drawing;
using Tesseract;

namespace PrototipoLeitorTesseract.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> RealizarLeitura(IFormFile image)
        {
            try
            {
                using var stream = new MemoryStream();
                await image.CopyToAsync(stream);

                using var engine = new TesseractEngine(@"wwwroot/tessdata", "por", EngineMode.Default);

                using var img = Pix.LoadFromMemory(stream.ToArray());
                using var page = engine.Process(img);

                var text = page.GetText().Replace("\n", "<br/>");

                return Json(new { success = true, result = text });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, result = ex.Message });
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
