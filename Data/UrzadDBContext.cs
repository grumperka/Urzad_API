using Microsoft.EntityFrameworkCore;
using Urzad_KSiwiak.Models;

namespace KSiwiak_Urzad_API.Data
{
    public class UrzadDBContext : DbContext
    {
        public UrzadDBContext(DbContextOptions options) : base(options)
        {
        }

        //DbSets

        public DbSet<Akty_rozwodu> Akty_rozwodu { get; set; }

        public DbSet<Akty_slubow> Akty_slubow { get; set; }

        public DbSet<Akty_urodzenia> Akty_urodzenia { get; set; }

        public DbSet<Akty_zgonu> Akty_zgonu { get; set; }

        public DbSet<Kierownicy> Kierownicy { get; set; }

        public DbSet<Konta_kierownikow> Konta_kierownikow { get; set; }

        public DbSet<Konta_obywateli> Konta_obywateli { get; set; }

        public DbSet<Konta_urzednikow> Konta_urzednikow { get; set; }

        public DbSet<Obywatele> Obywatele { get; set; }

        public DbSet<Powody_rozwodu> Powody_rozwodu { get; set; }

        public DbSet<Urzednicy> Urzednicy { get; set; }

        public DbSet<Urzedy> Urzedy { get; set; }

        public DbSet<Wojewodztwa> Wojewodztwa { get; set; }

       
    }
}
