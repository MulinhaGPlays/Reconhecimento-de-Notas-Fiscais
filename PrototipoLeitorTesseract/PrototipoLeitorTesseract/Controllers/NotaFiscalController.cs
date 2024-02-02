using Microsoft.AspNetCore.Mvc;
using PrototipoLeitorTesseract.Extensions;
using PrototipoLeitorTesseract.Models;
using PrototipoLeitorTesseract.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PrototipoLeitorTesseract.Controllers
{
    [Route("api/v1/nota-fiscal")]
    [ApiController]
    public class NotaFiscalController : ControllerBase
    {
        private readonly ImageService _imgService;
        private readonly TesseractService _tesService;

        public NotaFiscalController()
        {
            _imgService = new ImageService();
            _tesService = new TesseractService();
        }

        [DisableRequestSizeLimit]
        [HttpPost("ler-imagem/base64")]
        public IActionResult LerImagem([FromBody] Base64ImageViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var bytes = _imgService.ImageFormatter(model.Base64String);
                    string text = _tesService.ReadImage(bytes);
                    return Ok(new NotaFiscalViewModel(text, false));
                }
                else
                {
                    return BadRequest(model);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
