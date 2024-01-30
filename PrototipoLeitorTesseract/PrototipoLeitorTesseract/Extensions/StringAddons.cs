using System.Text;

namespace PrototipoLeitorTesseract.Extensions
{
    public static class StringAddons
    {
        public static string AddTesseractWhiteListCharacters(this string text)
            => text.AddUpperAlphabet().AddLowerAlphabet().AddAccentVogals().AddNumbers().AddSpecialCharacters(" .,-$()|/:;");

        public static string AddUpperAlphabet(this string text) => text + "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        public static string AddLowerAlphabet(this string text) => text + "abcdefghijklmnopqrstuvwxyz";
        
        public static string AddAccentVogals(this string text) => text + "áéíóúâêôÂÊÔÁÉÍÓÚãõÃÕ";

        public static string AddNumbers(this string text) => text + "0123456789";

        public static string AddSpecialCharacters(this string text, string characters) => text + characters;
    }
}
