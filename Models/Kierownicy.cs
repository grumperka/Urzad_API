using System.ComponentModel.DataAnnotations;

namespace Urzad_KSiwiak.Models
{
    public class Kierownicy
    {
        [Key]
        public int id { get; set; }

        [Required]
        [MaxLength(50)]
        public string imie { get; set; }

        [Required]
        [MaxLength(100)]
        public string nazwisko { get; set; }

        [Required]
        public int urzad_id { get; set; }

        [Required]
        public int czy_zastepca_T_N { get; set; }

        [Required]
        [MaxLength(100)]
        public string login { get; set; }
    }
}
