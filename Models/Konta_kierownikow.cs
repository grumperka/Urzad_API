using System.ComponentModel.DataAnnotations;

namespace Urzad_KSiwiak.Models
{
    public class Konta_kierownikow
    {
        [Key]
        public int id { get; set; }

        [Required]
        public int id_kierownika { get; set; }

        [Required]
        [MaxLength(100)]
        public string email { get; set; }

        [Required]
        [MaxLength(60)]
        public string haslo { get; set; }
    }
}
