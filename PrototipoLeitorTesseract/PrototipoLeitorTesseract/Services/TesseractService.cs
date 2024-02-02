using PrototipoLeitorTesseract.Extensions;
using Tesseract;

namespace PrototipoLeitorTesseract.Services
{
    public class TesseractService
    {
        private readonly TesseractEngine _engine;

        public TesseractService(string path = @"wwwroot\tessdata", string language = "por")
        {
            _engine = new TesseractEngine(path, language, EngineMode.Default);
        }

        public string ReadImage(byte[] bytes)
        {
            var img = Pix.LoadFromMemory(bytes);
            img = this.ConfigureImage(img);
            _engine.SetVariable("tessedit_char_whitelist", String.Empty.AddTesseractWhiteListCharacters());
            var page = _engine.Process(img, PageSegMode.SingleBlock);
            string text = page.GetText();
            return text;
        }

        public Pix ConfigureImage(Pix image)
        {
            image.Deskew();
            return image;
        }
    }
}
