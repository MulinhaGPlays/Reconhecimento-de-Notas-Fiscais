using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV;
using System.Drawing;
using System.Drawing.Imaging;
using Emgu.CV.Util;
using Emgu.CV.Dnn;

namespace PrototipoLeitorTesseract.Services
{
    public class ImageService
    {
        public async Task<byte[]> IFormFileReader(IFormFile image)
        {
            var stream = new MemoryStream();
            await image.CopyToAsync(stream);
            return stream.ToArray();
        }

        public Mat BytesToMat(byte[] bytes)
        {
            var img = new Mat();
            CvInvoke.Imdecode(bytes, ImreadModes.Color, img);
            return img;
        }
        public byte[] MatToBytes(Mat image)
        {
            var vector = new VectorOfByte();
            CvInvoke.Imencode(".png", image, vector);
            return vector.ToArray();
        }

        public async Task<byte[]> ImageFormatter(IFormFile iformFileImage)
        {
            var bytes = await this.IFormFileReader(iformFileImage);
            return this.ImageFactory(bytes);
        }
        public byte[] ImageFormatter(byte[] bytesImage) => this.ImageFactory(bytesImage);
        public byte[] ImageFormatter(string base64image)
        {
            byte[] image = Convert.FromBase64String(base64image);
            return this.ImageFactory(image);
        }

        public byte[] ImageFactory(byte[] image)
        {
            var mat = this.BytesToMat(image);
            var imagemMelhorada = VerificarEAtualizarQualidade(mat);
            return this.MatToBytes(imagemMelhorada);
        }

        public Mat VerificarEAtualizarQualidade(Mat imagem, double min = -0.0006, double max = 0.003)
        {
            //double nitidez = CalcularMetricaNitidez(imagem);
            imagem = MelhorarNitidez(imagem);
            imagem = MelhorarQualidade(imagem);
            return imagem;
        }

        public double CalcularMetricaNitidez(Mat imagem)
        {
            var resultado = new Mat();
            CvInvoke.Laplacian(imagem, resultado, DepthType.Cv64F);
            MCvScalar mean = CvInvoke.Mean(resultado);
            return mean.V0;
        }

        public Mat MelhorarNitidez(Mat imagem)
        {
            var imagemMelhorada = new Mat();
            int novaLargura = imagem.Width * 2;
            int novaAltura = imagem.Height * 2;

            CvInvoke.Resize(imagem, imagemMelhorada, new Size(novaLargura, novaAltura), interpolation: Inter.Linear);
            return imagemMelhorada;
        }
        public Mat MelhorarQualidade(Mat imagem)
        {
            CvInvoke.GaussianBlur(imagem, imagem, new Size(3, 3), 0);
            CvInvoke.Dilate(imagem, imagem, null, new Point(-1, -1), 1, BorderType.Default, new MCvScalar(1));
            CvInvoke.Erode(imagem, imagem, null, new Point(-1, -1), 1, BorderType.Default, new MCvScalar(1));

            //CvInvoke.CvtColor(imagem, imagem, ColorConversion.Bgr2Gray);
            //CvInvoke.Threshold(imagem, imagem, 0, 255, ThresholdType.Otsu);

            return imagem;
        }
    }
}
