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
    public class KierownicyController : ControllerBase
    {
        private readonly UrzadDBContext _context;

        public KierownicyController(UrzadDBContext context)
        {
            _context = context;
        }

        // GET: api/Kierownicy
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Kierownicy>>> GetKierownicy()
        {
            return await _context.Kierownicy.ToListAsync();
        }

        // GET: api/Kierownicy/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Kierownicy>> GetKierownicy(int id)
        {
            var kierownicy = await _context.Kierownicy.FindAsync(id);

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

            _context.Entry(kierownicy).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
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
            _context.Kierownicy.Add(kierownicy);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetKierownicy", new { id = kierownicy.id }, kierownicy);
        }

        // DELETE: api/Kierownicy/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteKierownicy(int id)
        {
            var kierownicy = await _context.Kierownicy.FindAsync(id);
            if (kierownicy == null)
            {
                return NotFound();
            }

            _context.Kierownicy.Remove(kierownicy);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool KierownicyExists(int id)
        {
            return _context.Kierownicy.Any(e => e.id == id);
        }
    }
}
