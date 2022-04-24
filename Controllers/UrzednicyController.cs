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
        private int index;

        public UrzednicyController(UrzadDBContext context)
        {
            _context = context;
            index = _context.Urzednicy.ToList().Last().id;
        }

        // GET: api/Urzednicy
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Urzednicy>>> GetUrzednicy()
        {
            return await _context.Urzednicy.ToListAsync();
        }

        // GET: api/Urzednicy/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Urzednicy>> GetUrzednicy(int id)
        {
            var urzednicy = await _context.Urzednicy.FindAsync(id);

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

            //_context.Entry(urzednicy).State = EntityState.Modified;

            try
            {
                Urzednicy urzednicyOld = _context.Urzednicy.Find(id);
                urzednicyOld.imie = urzednicy.imie;
                urzednicyOld.nazwisko = urzednicy.nazwisko;
                if (urzednicyOld.urzad_id != urzednicy.urzad_id) { 
                    int kierownikId = _context.Kierownicy.Where(w => w.urzad_id == urzednicy.urzad_id && w.czy_zastepca_T_N == 0).FirstOrDefault().id;
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
                await _context.SaveChangesAsync();
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
            this.index = this.index + 1;
            int kierownikId = _context.Kierownicy.Where(w => w.urzad_id == urzednicy.urzad_id && w.czy_zastepca_T_N == 0).FirstOrDefault().id;
            Urzednicy newUrzednicy = new Urzednicy { id = this.index, imie = urzednicy.imie, nazwisko = urzednicy.nazwisko, urzad_id = urzednicy.urzad_id, kierownik_id = kierownikId, login = urzednicy.imie.ToLower() + urzednicy.nazwisko + "_" + urzednicy.id+"@poczta.pl" };
            _context.Urzednicy.Add(newUrzednicy);
            await _context.SaveChangesAsync();

            return newUrzednicy;
        }

        // DELETE: api/Urzednicy/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUrzednicy(int id)
        {
            var urzednicy = await _context.Urzednicy.FindAsync(id);
            if (urzednicy == null)
            {
                return NotFound();
            }

            _context.Urzednicy.Remove(urzednicy);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UrzednicyExists(int id)
        {
            return _context.Urzednicy.Any(e => e.id == id);
        }
    }
}
