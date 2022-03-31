using System.ComponentModel.DataAnnotations;

namespace Urzad_KSiwiak.Models
{
    public class Urzedy
    {
        [Key]
        public int id { get; set; }

        [Required]
        [MaxLength(50)]
        public string nazwa_urzedu { get; set; }

        [Required]
        public int wojewodztwo_id { get; set; }
    }
}
