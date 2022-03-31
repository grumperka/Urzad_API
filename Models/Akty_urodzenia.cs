using System;
using System.ComponentModel.DataAnnotations;

namespace Urzad_KSiwiak.Models
{
    public class Akty_urodzenia
    {
        [Key]
        public int id { get; set; }

        [Required]
        public int id_obywatela { get; set; }

        [Required]
        public int id_ojca { get; set; }

        [Required]
        public int id_matki { get; set; }

        [Required]
        public int id_urzedu { get; set; }

        [Required]
        public int id_urzednika { get; set; }

        [Required]
        public DateTime data_wydania_aktu { get; set; }

    }
}
