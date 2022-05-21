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

namespace KSiwiak_Urzad_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Akty_urodzeniaController : ControllerBase
    {
        private readonly UrzadDBContext _context;

        public Akty_urodzeniaController(UrzadDBContext context)
        {
            _context = context;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public UrzadDBContext getContext(string header) {

            if (!header.Equals(""))
            {
                var contextOptions = new DbContextOptionsBuilder<UrzadDBContext>()
                .UseSqlServer("Data Source = GRUMPERKA\\SIWIAK; Initial Catalog = Urzedy; Integrated Security = False;" + header).Options;
                return new UrzadDBContext(contextOptions);
            }
            else {
                return this._context;
            }

        }

        // GET: api/Akty_urodzenia
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Akty_urodzenia>>> GetAkty_urodzenia()
        {
            string header = _context.getAuthorizationHeader(HttpContext);
            var context = getContext(header);

            List<Akty_urodzenia> results = new List<Akty_urodzenia>();

            try
            {
                results = await context.Akty_urodzenia.ToListAsync();
            }
            catch (SqlException ex) 
            {
                return Forbid(); //i tak nie zwraca :(
            }

            return results;
        }

        // GET: api/Akty_urodzenia/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Akty_urodzenia>> GetAkty_urodzenia(int id)
        {
            string header = _context.getAuthorizationHeader(HttpContext);
            var context = getContext(header);
            var akty_urodzenia = await context.Akty_urodzenia.FindAsync(id);

           if (akty_urodzenia == null)
           {
              return NotFound();
           }

           return akty_urodzenia;
        }


        [HttpGet("getAkt_urodzeniaFromUser/{id}")]
        public async Task<ActionResult<List<Akty_urodzenia>>> getAkt_urodzeniaFromUser(int id)
        {
            string header = _context.getAuthorizationHeader(HttpContext);
            var context = getContext(header);
            var akty_urodzenia = await context.Akty_urodzenia.Where(w => w.id_obywatela == id || w.id_ojca == id || w.id_matki == id).ToListAsync();

            if (akty_urodzenia == null)
            {
                return NotFound();
            }
            return akty_urodzenia;
        }

        [HttpGet("getAkt_urodzeniaFromUrzednik/{id}")]
        public async Task<ActionResult<List<Akty_urodzenia>>> GetAkt_urodzeniaFromUrzednik(int id)
        {
            string header = _context.getAuthorizationHeader(HttpContext);
            var context = getContext(header);
            var akty_urodzenia = await context.Akty_urodzenia.Where(w => w.id_urzednika == id).ToListAsync();

            if (akty_urodzenia == null)
            {
                return NotFound();
            }

            return akty_urodzenia;
        }

        [HttpGet("getAkt_urodzeniaFromUrzad/{id}")]
        public async Task<ActionResult<List<Akty_urodzenia>>> GetAkt_urodzeniaFromUrzad(int id)
        {
            string header = _context.getAuthorizationHeader(HttpContext);
            var context = getContext(header);
            int urzadID = context.Kierownicy.Find(id).urzad_id;
            if (urzadID != null)
            {
                var akty_urodzenia = await context.Akty_urodzenia.Where(w => w.id_urzedu == urzadID).ToListAsync();

                if (akty_urodzenia == null)
                {
                    return NotFound();
                }

                return akty_urodzenia;
            }
            return NotFound();
        }


        // POST: api/Akty_urodzenia
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Akty_urodzenia>> PostAkty_urodzenia(Akty_urodzenia akty_urodzenia)
        {
            string header = _context.getAuthorizationHeader(HttpContext);
            var context = getContext(header);
            akty_urodzenia.id = context.Akty_urodzenia.ToList().Last().id + 1;
            akty_urodzenia.id_urzedu = context.Urzednicy.Find(akty_urodzenia.id_urzednika).urzad_id;
            context.Akty_urodzenia.Add(akty_urodzenia);
            await context.SaveChangesAsync();

            return akty_urodzenia;
        }

        // DELETE: api/Akty_urodzenia/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAkty_urodzenia(int id)
        {
            string header = _context.getAuthorizationHeader(HttpContext);
            var context = getContext(header);
            var akty_urodzenia = await context.Akty_urodzenia.FindAsync(id);
            if (akty_urodzenia == null)
            {
                return NotFound();
            }

            context.Akty_urodzenia.Remove(akty_urodzenia);
            await context.SaveChangesAsync();

            return NoContent();
        }

        private bool Akty_urodzeniaExists(int id)
        {
            string header = _context.getAuthorizationHeader(HttpContext);
            var context = getContext(header);
            return context.Akty_urodzenia.Any(e => e.id == id);
        }
    }
}
