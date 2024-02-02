using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PrototipoLeitorTesseract.Extensions;
using PrototipoLeitorTesseract.Models;
using PrototipoLeitorTesseract.Services;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using Tesseract;

namespace PrototipoLeitorTesseract.Controllers
{
    public class HomeController : Controller
    {
        private readonly ImageService _imgService;
        private readonly TesseractService _tesService;

        public HomeController()
        {
            _imgService = new ImageService();
            _tesService = new TesseractService();
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
                var bytes = await _imgService.ImageFormatter(image);
                string text = _tesService.ReadImage(bytes);
                return Json(new NotaFiscalViewModel(text, true));
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, result = ex.Message });
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
