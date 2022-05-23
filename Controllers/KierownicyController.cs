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
using KSiwiak_Urzad_API.Models;
using Microsoft.Data.SqlClient;

namespace KSiwiak_Urzad_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KierownicyController : ControllerBase
    {
        private UrzadDBContext _context;

        public KierownicyController(UrzadDBContext context)
        {
            _context = context;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public UrzadDBContext getContext(string header)
        {

            if (!header.Equals(""))
            {
                var contextOptions = new DbContextOptionsBuilder<UrzadDBContext>()
                .UseSqlServer("Data Source = GRUMPERKA\\SIWIAK; Initial Catalog = Urzedy; Integrated Security = False;" + header).Options;
                return new UrzadDBContext(contextOptions);
            }
            else
            {
                return this._context;
            }
        }

            // GET: api/Kierownicy
            [HttpGet]
        public async Task<ActionResult<IEnumerable<Kierownicy>>> GetKierownicy()
        {
            string header = _context.getAuthorizationHeader(HttpContext);
            var context = getContext(header);
            return await context.Kierownicy.ToListAsync();
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<IEnumerable<Kierownicy_Urzad>>> GetKierownicy_Urzad()
        {

            string header = _context.getAuthorizationHeader(HttpContext);
            var context = getContext(header);

            var kierownicyList =  await context.Kierownicy.Where(w => w.czy_zastepca_T_N == 0).ToListAsync();
            var UrzedyList = await context.Urzedy.ToListAsync();

            List<Kierownicy_Urzad> kierownicy_Urzad_list = new List<Kierownicy_Urzad>();

            foreach (Kierownicy k in kierownicyList) {
                    string nazwaUrzedu = UrzedyList.Where(w => w.id == k.urzad_id).FirstOrDefault().nazwa_urzedu;

                    if (nazwaUrzedu != null) { 
                    kierownicy_Urzad_list.Add(new Kierownicy_Urzad { id = k.id, imie = k.imie, nazwisko = k.nazwisko, urzad_id = k.urzad_id, nazwa_urzedu = nazwaUrzedu, czy_zastepca_T_N = k.czy_zastepca_T_N, login = k.login } );
                    }
            }

            return kierownicy_Urzad_list;

        }

        // GET: api/Kierownicy/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Kierownicy>> GetKierownicy(int id)
        {
            string header = _context.getAuthorizationHeader(HttpContext);
            var context = getContext(header);

            var kierownicy = await context.Kierownicy.FindAsync(id);

            if (kierownicy == null)
            {
                return NotFound();
            }

            return kierownicy;
        }

        // PUT: api/Kierownicy/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutKierownicy(int id, Kierownicy kierownicy)
        {
            if (id != kierownicy.id)
            {
                return BadRequest();
            }

            string header = _context.getAuthorizationHeader(HttpContext);
            var context = getContext(header);

            try
            {
                Kierownicy kierownicyOld = context.Kierownicy.Find(id);
                kierownicyOld.imie = kierownicy.imie;
                kierownicyOld.nazwisko = kierownicy.nazwisko;
                kierownicyOld.urzad_id = kierownicy.urzad_id;
                kierownicyOld.czy_zastepca_T_N = kierownicy.czy_zastepca_T_N;
                kierownicyOld.login = kierownicy.imie.ToLower() + kierownicy.nazwisko + "_" + id + "@poczta.pl";
            }
            catch (ArgumentNullException ex)
            {
                return NotFound();
            }

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!KierownicyExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Kierownicy
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Kierownicy>> PostKierownicy(Kierownicy kierownicy)
        {
            string header = _context.getAuthorizationHeader(HttpContext);
            var context = getContext(header);

            kierownicy.id = context.Kierownicy.ToList().Last().id + 1;
            kierownicy.login = kierownicy.imie.ToLower() + kierownicy.nazwisko + "_" + kierownicy.id + "@poczta.pl";
            context.Kierownicy.Add(kierownicy);
            await context.SaveChangesAsync();

            return kierownicy;
        }

        // DELETE: api/Kierownicy/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteKierownicy(int id)
        {
            string header = _context.getAuthorizationHeader(HttpContext);
            var context = getContext(header);

            var kierownicy = await context.Kierownicy.FindAsync(id);
            if (kierownicy == null)
            {
                return NotFound();
            }

            context.Kierownicy.Remove(kierownicy);
            await context.SaveChangesAsync();

            return NoContent();
        }

        private bool KierownicyExists(int id)
        {
            string header = _context.getAuthorizationHeader(HttpContext);
            var context = getContext(header);
            return context.Kierownicy.Any(e => e.id == id);
        }
    }
}
