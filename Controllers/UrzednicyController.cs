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

namespace KSiwiak_Urzad_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UrzednicyController : ControllerBase
    {
        private readonly UrzadDBContext _context;

        public UrzednicyController(UrzadDBContext context)
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

        // GET: api/Urzednicy
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Urzednicy>>> GetUrzednicy()
        {
            string header = _context.getAuthorizationHeader(HttpContext);
            var context = getContext(header);
            return await context.Urzednicy.ToListAsync();
        }

        // GET: api/Urzednicy/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Urzednicy>> GetUrzednicy(int id)
        {
            string header = _context.getAuthorizationHeader(HttpContext);
            var context = getContext(header);

            var urzednicy = await context.Urzednicy.FindAsync(id);

            if (urzednicy == null)
            {
                return NotFound();
            }

            return urzednicy;
        }

        // PUT: api/Urzednicy/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUrzednicy(int id, Urzednicy urzednicy)
        {
            if (id != urzednicy.id)
            {
                return BadRequest();
            }

            string header = _context.getAuthorizationHeader(HttpContext);
            var context = getContext(header);

            try
            {
                Urzednicy urzednicyOld = context.Urzednicy.Find(id);
                urzednicyOld.imie = urzednicy.imie;
                urzednicyOld.nazwisko = urzednicy.nazwisko;
                if (urzednicyOld.urzad_id != urzednicy.urzad_id) { 
                    int kierownikId = context.Kierownicy.Where(w => w.urzad_id == urzednicy.urzad_id && w.czy_zastepca_T_N == 0).FirstOrDefault().id;
                    urzednicyOld.kierownik_id = kierownikId;
                    urzednicyOld.urzad_id = urzednicy.urzad_id;
                }
                urzednicyOld.login = urzednicy.imie.ToLower() + urzednicy.nazwisko + "_" + urzednicy.id + "@poczta.pl";
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
                if (!UrzednicyExists(id))
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

        // POST: api/Urzednicy
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Urzednicy>> PostUrzednicy(Urzednicy urzednicy)
        {
            string header = _context.getAuthorizationHeader(HttpContext);
            var context = getContext(header);

            int index = context.Urzednicy.ToList().Last().id + 1;
            int kierownikId = context.Kierownicy.Where(w => w.urzad_id == urzednicy.urzad_id && w.czy_zastepca_T_N == 0).FirstOrDefault().id;
            Urzednicy newUrzednicy = new Urzednicy { id = index, imie = urzednicy.imie, nazwisko = urzednicy.nazwisko, urzad_id = urzednicy.urzad_id, kierownik_id = kierownikId, login = urzednicy.imie.ToLower() + urzednicy.nazwisko + "_" + urzednicy.id+"@poczta.pl" };
            context.Urzednicy.Add(newUrzednicy);
            await context.SaveChangesAsync();

            return newUrzednicy;
        }

        // DELETE: api/Urzednicy/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUrzednicy(int id)
        {
            string header = _context.getAuthorizationHeader(HttpContext);
            var context = getContext(header);

            var urzednicy = await context.Urzednicy.FindAsync(id);
            if (urzednicy == null)
            {
                return NotFound();
            }

            context.Urzednicy.Remove(urzednicy);
            await context.SaveChangesAsync();

            return NoContent();
        }

        private bool UrzednicyExists(int id)
        {
            string header = _context.getAuthorizationHeader(HttpContext);
            var context = getContext(header);
            return context.Urzednicy.Any(e => e.id == id);
        }
    }
}
