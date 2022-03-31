using System;
using System.ComponentModel.DataAnnotations;

namespace Urzad_KSiwiak.Models
{
    public class Akty_rozwodu
    {
        [Key]
        public int id { get; set; }

        [Required]
        public int id_rozwodnika { get; set; }

        [Required]
        public int id_rozwodniczki { get; set; }

        [Required]
        public int id_urzedu { get; set; }

        [Required]
        public int id_urzednika { get; set; }

        [Required]
        public DateTime data_slubu { get; set; }

        [Required]
        public DateTime data_wydania_aktu_rozwodu { get; set; }

        [Required]
        public int z_orzekaniem_winy_T_N { get; set; }

        [Required]
        public int id_powodu_glownego { get; set; }

        [Required]
        public int czy_wylacznie_T_N { get; set; }
    }
}
