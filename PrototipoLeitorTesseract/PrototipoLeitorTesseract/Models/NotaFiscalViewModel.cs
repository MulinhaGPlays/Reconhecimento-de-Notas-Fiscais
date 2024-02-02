using PrototipoLeitorTesseract.Extensions;

namespace PrototipoLeitorTesseract.Models
{
    public class NotaFiscalViewModel
    {
        private readonly string _leituraCompleta = String.Empty;
        private readonly bool _html = false;

        public NotaFiscalViewModel(string text, bool html)
        {
            _leituraCompleta = text;
            _html = html;
        }

        public string LeituraCompleta => _html ? _leituraCompleta.TrocarQuebrasDeLinhaPorHtmlBr() : _leituraCompleta;
        public string CNPJ => _leituraCompleta.RegexPegarCNPJ();
        public string NumeroCupom => _leituraCompleta.RegexPegarNumeroCupom();
        public string ChaveCupom => _leituraCompleta.RegexChaveCupom();
        public string DataHoraCompra => _leituraCompleta.RegexDataHoraCompra();
        public string[] Produtos => _leituraCompleta.RegexPegarDadosProdutos();
    }
}
