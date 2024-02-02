using System.ComponentModel.DataAnnotations;

namespace PrototipoLeitorTesseract.Models
{
    public class Base64ImageViewModel
    {
        [Required(ErrorMessage = $"O Campo {nameof(Base64String)} é obrigatório.")]
        public string Base64String { get; set; } = String.Empty;
    }
}
