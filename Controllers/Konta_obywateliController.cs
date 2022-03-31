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
    public class Konta_obywateliController : ControllerBase
    {
        private readonly UrzadDBContext _context;

        public Konta_obywateliController(UrzadDBContext context)
        {
            _context = context;
        }

        // GET: api/Konta_obywateli
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Konta_obywateli>>> GetKonta_obywateli()
        {
            return await _context.Konta_obywateli.ToListAsync();
        }

        // GET: api/Konta_obywateli/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Konta_obywateli>> GetKonta_obywateli(int id)
        {
            var konta_obywateli = await _context.Konta_obywateli.FindAsync(id);

            if (konta_obywateli == null)
            {
                return NotFound();
            }

            return konta_obywateli;
        }

        // PUT: api/Konta_obywateli/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutKonta_obywateli(int id, Konta_obywateli konta_obywateli)
        {
            if (id != konta_obywateli.id)
            {
                return BadRequest();
            }

            _context.Entry(konta_obywateli).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Konta_obywateliExists(id))
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

        // POST: api/Konta_obywateli
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Konta_obywateli>> PostKonta_obywateli(Konta_obywateli konta_obywateli)
        {
            _context.Konta_obywateli.Add(konta_obywateli);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetKonta_obywateli", new { id = konta_obywateli.id }, konta_obywateli);
        }

        // DELETE: api/Konta_obywateli/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteKonta_obywateli(int id)
        {
            var konta_obywateli = await _context.Konta_obywateli.FindAsync(id);
            if (konta_obywateli == null)
            {
                return NotFound();
            }

            _context.Konta_obywateli.Remove(konta_obywateli);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool Konta_obywateliExists(int id)
        {
            return _context.Konta_obywateli.Any(e => e.id == id);
        }
    }
}
