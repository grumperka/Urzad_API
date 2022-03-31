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
    public class Konta_kierownikowController : ControllerBase
    {
        private readonly UrzadDBContext _context;

        public Konta_kierownikowController(UrzadDBContext context)
        {
            _context = context;
        }

        // GET: api/Konta_kierownikow
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Konta_kierownikow>>> GetKonta_kierownikow()
        {
            return await _context.Konta_kierownikow.ToListAsync();
        }

        // GET: api/Konta_kierownikow/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Konta_kierownikow>> GetKonta_kierownikow(int id)
        {
            var konta_kierownikow = await _context.Konta_kierownikow.FindAsync(id);

            if (konta_kierownikow == null)
            {
                return NotFound();
            }

            return konta_kierownikow;
        }

        // PUT: api/Konta_kierownikow/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutKonta_kierownikow(int id, Konta_kierownikow konta_kierownikow)
        {
            if (id != konta_kierownikow.id)
            {
                return BadRequest();
            }

            _context.Entry(konta_kierownikow).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Konta_kierownikowExists(id))
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

        // POST: api/Konta_kierownikow
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Konta_kierownikow>> PostKonta_kierownikow(Konta_kierownikow konta_kierownikow)
        {
            _context.Konta_kierownikow.Add(konta_kierownikow);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetKonta_kierownikow", new { id = konta_kierownikow.id }, konta_kierownikow);
        }

        // DELETE: api/Konta_kierownikow/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteKonta_kierownikow(int id)
        {
            var konta_kierownikow = await _context.Konta_kierownikow.FindAsync(id);
            if (konta_kierownikow == null)
            {
                return NotFound();
            }

            _context.Konta_kierownikow.Remove(konta_kierownikow);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool Konta_kierownikowExists(int id)
        {
            return _context.Konta_kierownikow.Any(e => e.id == id);
        }
    }
}
