using System;
using System.ComponentModel.DataAnnotations;

namespace Urzad_KSiwiak.Models
{
    public class Akty_slubow
    {
        [Key]
        public int id { get; set; }

        [Required]
        public int id_malzonka { get; set; }

        [Required]
        public int id_malzonki { get; set; }

        [Required]
        public int id_urzedu { get; set; }

        [Required]
        public int id_urzednika { get; set; }

        [Required]
        public int id_swiadka_1 { get; set; }

        [Required]
        public int id_swiadka_2 { get; set; }

        [Required]
        public DateTime data_wydania_aktu { get; set; }
    }
}
