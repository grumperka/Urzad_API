using KSiwiak_Urzad_API.Models;
using Microsoft.EntityFrameworkCore;
using Urzad_KSiwiak.Models;

namespace KSiwiak_Urzad_API.Data
{
    public class UrzadDBContext : DbContext
    {
        public UrzadDBContext(DbContextOptions<UrzadDBContext> options) : base(options)
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

        //public bool isSessionOpen(HttpContext httpContext)
        //{
        //    byte[] tokenb = new Byte[20];
        //    byte[] rolab = new Byte[10];
        //    byte[] userIDb = new Byte[10];

        //    bool isToken = httpContext.Session.TryGetValue("token", out tokenb);
        //    bool isRola = httpContext.Session.TryGetValue("rola", out rolab);
        //    bool isUserID = httpContext.Session.TryGetValue("userID", out userIDb);

        //    if (isToken || isRola || isUserID)
        //    {
        //        return true;
        //    }
        //    else return false;
        //}

        //public Token isTokenValid(HttpContext httpContext, string tokenString) 
        //{
        //    string token = httpContext.Session.GetString("token");
        //    string rola = httpContext.Session.GetString("rola");
        //    int userID = (int)httpContext.Session.GetInt32("userID");

        //    if (token.Equals(tokenString)) {
        //        return new Token { token = token, rola = rola, userID = userID };
        //    }
        //    else return null;
        //}

        public string getRola(HttpContext httpContext)
        {
            return httpContext.Session.GetString("rola");
        }

        public int getUserId(HttpContext httpContext)
        {
            return (int)httpContext.Session.GetInt32("userID");
        }

        public string getAuthorizationHeader(HttpContext httpContext) { 
            return httpContext.Request.Headers["Authorization"].ToString();
        }

    }
}
