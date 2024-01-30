using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV;
using System.Drawing;

namespace PrototipoLeitorTesseract.Services
{
    public class ImageService
    {
        public async Task<byte[]> IFormFileReader(IFormFile image)
        {
            using var stream = new MemoryStream();
            await image.CopyToAsync(stream);
            return stream.ToArray();
        }

        public async Task<Image<Bgr, byte>?> ImageResizer(IFormFile image)
        {
            var imageBytes = await this.IFormFileReader(image);

            var imagemOriginal = new Mat();
            CvInvoke.Imdecode(imageBytes, ImreadModes.AnyColor, imagemOriginal);

            int novaLargura = imagemOriginal.Width * 2;
            int novaAltura = imagemOriginal.Height * 2;

            var imagemMelhorada = new Mat();

            CvInvoke.Resize(imagemOriginal, imagemMelhorada, new Size(novaLargura, novaAltura), interpolation: Inter.Linear);
            CvInvoke.GaussianBlur(imagemMelhorada, imagemMelhorada, new Size(3, 3), 0);
            CvInvoke.Dilate(imagemMelhorada, imagemMelhorada, null, new Point(-1, -1), 1, BorderType.Default, new MCvScalar(1));
            CvInvoke.Erode(imagemMelhorada, imagemMelhorada, null, new Point(-1, -1), 1, BorderType.Default, new MCvScalar(1));

            return imagemMelhorada.ToImage<Bgr, byte>();
        }

        public async Task<string> ImageSaver(IFormFile image, string path = @"wwwroot\images\temp_image.png")
        {
            using var rezedImage = await this.ImageResizer(image);
            rezedImage?.Save(path);
            return path;
        }

        public void DeleteImage(string path = @"wwwroot\images\temp_image.png")
        {
            File.Delete(path);
        }
    }
}
