using System.Text.RegularExpressions;

namespace PrototipoLeitorTesseract.Extensions
{
    public static class RegexAddons
    {
        static string ExtractData(string input, string pattern)
        {
            var regex = new Regex(pattern);
            var match = regex.Match(input);
            return match.Success ? match.Value : "Não Encontrado";
        }

        public static string RegexCNPJ(this string input, string pattern = @"(([A-Za-z]{4}[-])?)\b\d{2}[.,]\d{3}[.,]\d{3}/\d{4}-\d{2}\b")
        {
            return ExtractData(input, pattern).Replace("CNPJ-", "");
        }
        
        public static string RegexChaveCupom(this string input, string pattern = @"\b\S{4,6}\s\S{4,6}\s\S{4,6}\s\S{4,6}\s\S{4,6}\s\S{4,6}\s\S{4,6}\s\S{4,6}\s\S{4,6}(\s\S{4,6})?(\s\S{4,6})?(\s\S{4,6})?\b")
        {
            return ExtractData(input, pattern);
        }
        
        public static string RegexNumeroCupom(this string input, string pattern = @"\b(No\. (\S+))|(([Nn ])?(:)?(u[nm]ero)? (\d{3}\.\d{3}\.\d{3}))\b")
        {
            return ExtractData(input, pattern);
        }
        
        public static string RegexDataHoraCompra(this string input, string pattern = @"\b\d{2}/\d{2}/\d{4} \d{2}:\d{2}:\d{2}\b")
        {
            return ExtractData(input, pattern);
        }
    }
}
