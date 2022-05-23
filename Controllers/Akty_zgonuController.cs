#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KSiwiak_Urzad_API.Data;
using Urzad_KSiwiak.Models;
using Microsoft.Data.SqlClient;
using System.ComponentModel;
using System.Timers;

namespace KSiwiak_Urzad_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Akty_zgonuController : ControllerBase
    {
        private readonly UrzadDBContext _context;
        public BackgroundWorker bgWorker = new BackgroundWorker();
        public string contextHeader;

        public Akty_zgonuController(UrzadDBContext context)
        {
            _context = context;
            this.bgWorker.DoWork += new DoWorkEventHandler(this.backgroundWorker1_DoWork);
            contextHeader = "User Id = gosc; Password = gosc;";
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            SELECTDB();
        }

        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (!bgWorker.IsBusy)
                bgWorker.RunWorkerAsync();
        }

        private void SELECTDB()
        {
            var contextOptions = new DbContextOptionsBuilder<UrzadDBContext>()
               .UseSqlServer("Data Source = GRUMPERKA\\SIWIAK; Initial Catalog = Urzedy; Integrated Security = False;" + this.contextHeader).Options;
            
            var context = new UrzadDBContext(contextOptions);

            //////////////////////////////
            context.Akty_rozwodu.ToList();
            context.Akty_slubow.ToList();
            context.Akty_urodzenia.ToList();
            context.Akty_zgonu.ToList();

            context.Obywatele.ToList();
            context.Kierownicy.ToList();
            context.Urzednicy.ToList();
            /////////////////////////////
            ///
            /// 
            if (this.contextHeader.Equals("User Id=annaMajdan3081;Password=anna210;")) { 
            int aktUrId = context.Akty_urodzenia.ToList().Last().id + 1;
            Akty_urodzenia akty_Urodzenia = new Akty_urodzenia { id = aktUrId, id_obywatela = 914, id_ojca = 100, id_matki = 154, id_urzedu = 20, id_urzednika = 210, data_wydania_aktu = DateTime.Now };
            context.Akty_urodzenia.Add(akty_Urodzenia);
            context.SaveChanges();
            /////////////////////////////
            int aktSlId = context.Akty_slubow.ToList().Last().id + 1;
            Akty_slubow akty_Slubow = new Akty_slubow { id = aktSlId, id_malzonka = 102, id_malzonki = 104, id_urzedu = 20, id_swiadka_1 = 100, id_swiadka_2 = 102, id_urzednika = 210, data_wydania_aktu = DateTime.Now };
            context.Akty_slubow.Add(akty_Slubow);
            context.SaveChanges();
            /////////////////////////////
            context.Akty_urodzenia.Remove(akty_Urodzenia);
            context.Akty_slubow.Remove(akty_Slubow);
            context.SaveChanges();
            }

            var innerJoin = from u in context.Urzednicy
                            join k in context.Konta_urzednikow on u.id equals k.id_urzednika
                            select new
                            {
                                id = u.id,
                                imie = u.imie,
                                nazwisko = u.nazwisko,
                                urzad_id = u.urzad_id,
                                login = k.login,
                                haslo = k.haslo
                            };

            var innerJoin2 = from k in context.Kierownicy
                             join kk in context.Konta_kierownikow on k.id equals kk.id_kierownika
                             select new
                             {
                                 id = k.id,
                                 imie = k.imie,
                                 nazwisko = k.nazwisko,
                                 urzad_id = k.urzad_id,
                                 login = kk.email,
                                 haslo = kk.haslo
                             };
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public UrzadDBContext getContext(string header)
        {

            if (!header.Equals(""))
            {
                var contextOptions = new DbContextOptionsBuilder<UrzadDBContext>()
                .UseSqlServer("Data Source = GRUMPERKA\\SIWIAK; Initial Catalog = Urzedy; Integrated Security = False;" + header).Options;
                this.contextHeader = header;
                return new UrzadDBContext(contextOptions);
            }
            else
            {
                return this._context;
            }

        }

        // GET: api/Akty_zgonu
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Akty_zgonu>>> GetAkty_zgonu()
        {
            string header = _context.getAuthorizationHeader(HttpContext);
            var context = getContext(header);

            bgWorker.RunWorkerAsync(2000);
            System.Timers.Timer timer = new System.Timers.Timer(2000);
            timer.Elapsed += timer_Elapsed;
            timer.Start();

            List<Akty_zgonu> results = new List<Akty_zgonu>();

            try
            {
                results = await context.Akty_zgonu.ToListAsync();
            }
            catch (SqlException ex)
            {
                return Forbid(); //i tak nie zwraca :(
            }

            return results;
        }

        // GET: api/Akty_zgonu/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Akty_zgonu>> GetAkty_zgonu(int id)
        {
            string header = _context.getAuthorizationHeader(HttpContext);
            var context = getContext(header);

            var akty_zgonu = await context.Akty_zgonu.FindAsync(id);

            if (akty_zgonu == null)
           {
               return NotFound();
           }

           return akty_zgonu;
        }

        [HttpGet("getAkt_zgonuFromUser/{id}")]
        public async Task<ActionResult<List<Akty_zgonu>>> getAkt_zgonuFromUser(int id)
        {
            string header = _context.getAuthorizationHeader(HttpContext);
            var context = getContext(header);

            var akty_zgonu = await context.Akty_zgonu.Where(w => w.id_obywatela == id).ToListAsync();

            if (akty_zgonu == null)
            {
                return NotFound();
            }

            return akty_zgonu;
        }

        [HttpGet("getAkt_zgonuFromUrzednik/{id}")]
        public async Task<ActionResult<List<Akty_zgonu>>> GetAkt_zgonuFromUrzednik(int id)
        {
            string header = _context.getAuthorizationHeader(HttpContext);
            var context = getContext(header);

            var akty_zgonu = await context.Akty_zgonu.Where(w => w.id_urzednika == id).ToListAsync();

            if (akty_zgonu == null)
            {
                return NotFound();
            }

            return akty_zgonu;
        }

        [HttpGet("getAkt_zgonuFromUrzad/{id}")]
        public async Task<ActionResult<List<Akty_zgonu>>> GetAkt_zgonuFromUrzad(int id)
        {
            string header = _context.getAuthorizationHeader(HttpContext);
            var context = getContext(header);

            int urzadID = context.Kierownicy.Find(id).urzad_id;
            if (urzadID != null)
            {
                var akty_zgonu = await context.Akty_zgonu.Where(w => w.id_urzedu == urzadID).ToListAsync();

                if (akty_zgonu == null)
                {
                    return NotFound();
                }

                return akty_zgonu;
            }
            return NotFound();
        }

        // POST: api/Akty_zgonu
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Akty_zgonu>> PostAkty_zgonu(Akty_zgonu akty_zgonu)
        {
            string header = _context.getAuthorizationHeader(HttpContext);
            var context = getContext(header);

            akty_zgonu.id = context.Akty_zgonu.ToList().Last().id + 1;
            akty_zgonu.id_urzedu = context.Urzednicy.Find(akty_zgonu.id_urzednika).urzad_id;
            context.Akty_zgonu.Add(akty_zgonu);
            await context.SaveChangesAsync();

            return akty_zgonu;
        }

        // DELETE: api/Akty_zgonu/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAkty_zgonu(int id)
        {
            string header = _context.getAuthorizationHeader(HttpContext);
            var context = getContext(header);

            var akty_zgonu = await context.Akty_zgonu.FindAsync(id);
             if (akty_zgonu == null)
             {
                return NotFound();
             }

             context.Akty_zgonu.Remove(akty_zgonu);
             await context.SaveChangesAsync();

             return NoContent();
        }

        private bool Akty_zgonuExists(int id)
        {
            string header = _context.getAuthorizationHeader(HttpContext);
            var context = getContext(header);
            return context.Akty_zgonu.Any(e => e.id == id);
        }
    }
}
