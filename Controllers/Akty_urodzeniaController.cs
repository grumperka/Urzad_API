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
    public class Akty_urodzeniaController : ControllerBase
    {
        private readonly UrzadDBContext _context;
        private int index;

        public Akty_urodzeniaController(UrzadDBContext context)
        {
            _context = context;
            index = _context.Akty_urodzenia.ToList().Last().id;
        }

        // GET: api/Akty_urodzenia
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Akty_urodzenia>>> GetAkty_urodzenia()
        {
            //if (_context.isSessionOpen(HttpContext))
            //{

            //    string rola = _context.getRola(HttpContext);

            //    if (rola.Equals("urzednik") || rola.Equals("kierownik"))
            //    {
            return await _context.Akty_urodzenia.ToListAsync();
            //    }
            //    else return Forbid();
            //}
            //else return Forbid();
        }

        // GET: api/Akty_urodzenia/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Akty_urodzenia>> GetAkty_urodzenia(int id)
        {
            //if (_context.isSessionOpen(HttpContext))
            //{
            //    string rola = _context.getRola(HttpContext);

            //    if (rola.Equals("urzednik") || rola.Equals("kierownik"))
            //    {
            var akty_urodzenia = await _context.Akty_urodzenia.FindAsync(id);

            if (akty_urodzenia == null)
            {
                return NotFound();
            }

            return akty_urodzenia;
            //    }
            //    else if (rola.Equals("obywatel")) {

            //        var akty_urodzenia = await _context.Akty_urodzenia.FindAsync(id);

            //        if (akty_urodzenia == null || akty_urodzenia.id_obywatela != _context.getUserId(HttpContext))
            //        {
            //            return NotFound();
            //        }

            //        return akty_urodzenia;
            //    }
            //    else return Forbid();
            //}
            //else return Forbid();
        }

        [HttpGet("getAkt_urodzeniaFromUser/{id}")]
        public async Task<ActionResult<List<Akty_urodzenia>>> getAkt_urodzeniaFromUser(int id)
        {
            var akty_urodzenia = await _context.Akty_urodzenia.Where(w => w.id_obywatela == id || w.id_ojca == id || w.id_matki == id).ToListAsync();

            if (akty_urodzenia == null)
            {
                return NotFound();
            }

            return akty_urodzenia;
        }

        [HttpGet("getAkt_urodzeniaFromUrzednik/{id}")]
        public async Task<ActionResult<List<Akty_urodzenia>>> GetAkt_urodzeniaFromUrzednik(int id)
        {
            var akty_urodzenia = await _context.Akty_urodzenia.Where(w => w.id_urzednika == id).ToListAsync();

            if (akty_urodzenia == null)
            {
                return NotFound();
            }

            return akty_urodzenia;
        }

        [HttpGet("getAkt_urodzeniaFromUrzad/{id}")]
        public async Task<ActionResult<List<Akty_urodzenia>>> GetAkt_urodzeniaFromUrzad(int id)
        {
            int urzadID = _context.Kierownicy.Find(id).urzad_id;
            if (urzadID != null)
            {
                var akty_urodzenia = await _context.Akty_urodzenia.Where(w => w.id_urzedu == urzadID).ToListAsync();

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
            //if (_context.isSessionOpen(HttpContext))
            //{

            //    string rola = _context.getRola(HttpContext);

            //    if (rola.Equals("urzednik") || rola.Equals("kierownik"))
            //    {
            this.index += 1;
            akty_urodzenia.id = this.index;
            akty_urodzenia.id_urzedu = _context.Urzednicy.Find(akty_urodzenia.id_urzednika).urzad_id;
            _context.Akty_urodzenia.Add(akty_urodzenia);
            await _context.SaveChangesAsync();

            return akty_urodzenia;
            //    }
            //    else return Forbid();
            //}
            //else return Forbid();
        }

        // DELETE: api/Akty_urodzenia/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAkty_urodzenia(int id)
        {
            //if (_context.isSessionOpen(HttpContext))
            //{
            //    string rola = _context.getRola(HttpContext);

            //    if (rola.Equals("urzednik") || rola.Equals("kierownik"))
            //    {
            var akty_urodzenia = await _context.Akty_urodzenia.FindAsync(id);
            if (akty_urodzenia == null)
            {
                return NotFound();
            }

            _context.Akty_urodzenia.Remove(akty_urodzenia);
            await _context.SaveChangesAsync();

            return NoContent();
            //    }
            //    else return Forbid();
            //}
            //else return Forbid();
        }

        private bool Akty_urodzeniaExists(int id)
        {
            return _context.Akty_urodzenia.Any(e => e.id == id);
        }
    }
}
