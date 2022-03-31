using System.ComponentModel.DataAnnotations;

namespace Urzad_KSiwiak.Models
{
    public class Obywatele
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
        [MaxLength(100)]
        public string nazwisko_rodowe { get; set; }

        [Required]
        public long pesel { get; set; }

        [Required]
        [MaxLength(1)]
        public string plec { get; set; }

        [Required]
        public int wojewodztwo_id { get; set; }
    }
}
