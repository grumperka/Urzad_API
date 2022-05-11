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
    public class Akty_slubowController : ControllerBase
    {
        private readonly UrzadDBContext _context;
        private int index;

        public Akty_slubowController(UrzadDBContext context)
        {
            _context = context;
            index = _context.Akty_slubow.ToList().Last().id;
        }

        // GET: api/Akty_slubow
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Akty_slubow>>> GetAkty_slubow()
        {
            //if (_context.isSessionOpen(HttpContext))
            //{

            //    string rola = _context.getRola(HttpContext);

            //    if (rola.Equals("urzednik") || rola.Equals("kierownik"))
            //    {
            return await _context.Akty_slubow.ToListAsync();
            //    }
            //    else return Forbid();
            //}
            //else return Forbid();
        }

        // GET: api/Akty_slubow/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Akty_slubow>> GetAkty_slubow(int id)
        {
            var akty_slubow = await _context.Akty_slubow.FindAsync(id);

            if (akty_slubow == null)
            {
                return NotFound();
            }

            return akty_slubow;
        }

        [HttpGet("getAkt_slubuFromUser/{id}")]
        public async Task<ActionResult<List<Akty_slubow>>> GetAkt_slubuFromUser(int id)
        {
            var akty_slubow = await _context.Akty_slubow.Where(w => w.id_malzonka == id || w.id_malzonki == id).ToListAsync();

            if (akty_slubow == null)
            {
                return NotFound();
            }

            return akty_slubow;
        }

        [HttpGet("getAkt_slubuFromUrzednik/{id}")]
        public async Task<ActionResult<List<Akty_slubow>>> GetAkt_slubuFromUrzednik(int id)
        {
            var akty_slubow = await _context.Akty_slubow.Where(w => w.id_urzednika == id).ToListAsync();

            if (akty_slubow == null)
            {
                return NotFound();
            }

            return akty_slubow;
        }

        [HttpGet("getAkt_slubuFromUrzad/{id}")]
        public async Task<ActionResult<List<Akty_slubow>>> GetAkt_slubuFromUrzad(int id)
        {
            int urzadID = _context.Kierownicy.Find(id).urzad_id;
            if (urzadID != null)
            {
                var akty_slubow = await _context.Akty_slubow.Where(w => w.id_urzedu == urzadID).ToListAsync();

                if (akty_slubow == null)
                {
                    return NotFound();
                }

                return akty_slubow;
            }
            return NotFound();
        }

        // POST: api/Akty_slubow
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Akty_slubow>> PostAkty_slubow(Akty_slubow akty_slubow)
        {
            //if (_context.isSessionOpen(HttpContext))
            //{

            //    string rola = _context.getRola(HttpContext);

            //    if (rola.Equals("urzednik") || rola.Equals("kierownik"))
            //    {
            this.index += 1;
            akty_slubow.id = this.index;
            akty_slubow.id_urzedu = _context.Urzednicy.Find(akty_slubow.id_urzednika).urzad_id;
            _context.Akty_slubow.Add(akty_slubow);
            await _context.SaveChangesAsync();

            return akty_slubow;
            //    }
            //    else return Forbid();
            //}
            //else return Forbid();
        }

        // DELETE: api/Akty_slubow/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAkty_slubow(int id)
        {
            //if (_context.isSessionOpen(HttpContext))
            //{

            //    string rola = _context.getRola(HttpContext);

            //    if (rola.Equals("urzednik") || rola.Equals("kierownik"))
            //    {
            var akty_slubow = await _context.Akty_slubow.FindAsync(id);
            if (akty_slubow == null)
            {
                return NotFound();
            }

            _context.Akty_slubow.Remove(akty_slubow);
            await _context.SaveChangesAsync();

            return NoContent();
            //    }
            //    else return Forbid();
            //}
            //else return Forbid();
        }

        private bool Akty_slubowExists(int id)
        {
            return _context.Akty_slubow.Any(e => e.id == id);
        }
    }
}
