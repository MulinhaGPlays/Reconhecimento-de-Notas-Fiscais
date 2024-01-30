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
        private readonly ILogger<HomeController> _logger;
        private readonly ImageService _imgService;
        private readonly TesseractService _tesService;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
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
                string path = await _imgService.ImageSaver(image);
                string text = _tesService.ReadImage(path);
                _imgService.DeleteImage();

                return Json(new 
                { 
                    success = true, 
                    result = text.Replace("\n", "<br/>"),
                    cnpj = text.RegexPegarCNPJ(),
                    chaveCupom = text.RegexChaveCupom(),
                    numeroCupom = text.RegexPegarNumeroCupom(),
                    dataHoraCompra = text.RegexDataHoraCompra(),
                    produtos = text.Replace("\n", " ").RegexPegarDadosProdutos()
                });
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
