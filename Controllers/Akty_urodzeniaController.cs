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

        public Akty_urodzeniaController(UrzadDBContext context)
        {
            _context = context;
        }

        // GET: api/Akty_urodzenia
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Akty_urodzenia>>> GetAkty_urodzenia()
        {
            return await _context.Akty_urodzenia.ToListAsync();
        }

        // GET: api/Akty_urodzenia/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Akty_urodzenia>> GetAkty_urodzenia(int id)
        {
            var akty_urodzenia = await _context.Akty_urodzenia.FindAsync(id);

            if (akty_urodzenia == null)
            {
                return NotFound();
            }

            return akty_urodzenia;
        }

        // PUT: api/Akty_urodzenia/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAkty_urodzenia(int id, Akty_urodzenia akty_urodzenia)
        {
            if (id != akty_urodzenia.id)
            {
                return BadRequest();
            }

            _context.Entry(akty_urodzenia).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Akty_urodzeniaExists(id))
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

        // POST: api/Akty_urodzenia
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Akty_urodzenia>> PostAkty_urodzenia(Akty_urodzenia akty_urodzenia)
        {
            _context.Akty_urodzenia.Add(akty_urodzenia);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAkty_urodzenia", new { id = akty_urodzenia.id }, akty_urodzenia);
        }

        // DELETE: api/Akty_urodzenia/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAkty_urodzenia(int id)
        {
            var akty_urodzenia = await _context.Akty_urodzenia.FindAsync(id);
            if (akty_urodzenia == null)
            {
                return NotFound();
            }

            _context.Akty_urodzenia.Remove(akty_urodzenia);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool Akty_urodzeniaExists(int id)
        {
            return _context.Akty_urodzenia.Any(e => e.id == id);
        }
    }
}
