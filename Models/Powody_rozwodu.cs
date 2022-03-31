using System.ComponentModel.DataAnnotations;

namespace Urzad_KSiwiak.Models
{
    public class Powody_rozwodu
    {
        [Key]
        public int id { get; set; }

        [Required]
        [MaxLength(50)]
        public string nazwa_powodu_rozwodu { get; set; }
    }
}
