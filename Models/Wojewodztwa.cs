using System.ComponentModel.DataAnnotations;

namespace Urzad_KSiwiak.Models
{
    public class Wojewodztwa
    {
        [Key]
        public int id { get; set; }

        [Required]
        [MaxLength(50)]
        public string nazwa { get; set; }
    }
}
