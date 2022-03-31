using System.ComponentModel.DataAnnotations;

namespace Urzad_KSiwiak.Models
{
    public class Konta_obywateli
    {
        [Key]
        public int id { get; set; }

        [Required]
        public int id_obywatela { get; set; }

        [Required]
        [MaxLength(100)]
        public string login { get; set; }

        [Required]
        [MaxLength(60)]
        public string haslo { get; set; }
    }
}
