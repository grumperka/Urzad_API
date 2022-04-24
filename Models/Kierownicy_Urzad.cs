using System.ComponentModel.DataAnnotations;

namespace KSiwiak_Urzad_API.Models
{
    public class Kierownicy_Urzad
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
        [MaxLength(50)]
        public string nazwa_urzedu { get; set; }

        [Required]
        public int czy_zastepca_T_N { get; set; }

        [Required]
        [MaxLength(100)]
        public string login { get; set; }
    }
}
