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

            _context.Entry(urzednicy).State = EntityState.Modified;

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
            _context.Urzednicy.Add(urzednicy);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUrzednicy", new { id = urzednicy.id }, urzednicy);
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
