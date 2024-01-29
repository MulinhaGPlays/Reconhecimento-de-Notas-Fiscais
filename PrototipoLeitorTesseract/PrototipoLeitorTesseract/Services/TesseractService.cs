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

        public string ReadImage(string path)
        {
            using var img = Pix.LoadFromFile(path);

            _engine.SetVariable("tessedit_char_whitelist", String.Empty
                .AddUpperAlphabet()
                .AddLowerAlphabet()
                .AddAccentVogals()
                .AddNumbers()
                .AddSpecialCharacters(" .,-$()|/:;"));

            using var page = _engine.Process(img, PageSegMode.SingleBlock);
            string text = page.GetText();

            return text;
        }
    }
}
