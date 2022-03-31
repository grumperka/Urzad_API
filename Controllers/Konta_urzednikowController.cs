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
    public class Konta_urzednikowController : ControllerBase
    {
        private readonly UrzadDBContext _context;

        public Konta_urzednikowController(UrzadDBContext context)
        {
            _context = context;
        }

        // GET: api/Konta_urzednikow
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Konta_urzednikow>>> GetKonta_urzednikow()
        {
            return await _context.Konta_urzednikow.ToListAsync();
        }

        // GET: api/Konta_urzednikow/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Konta_urzednikow>> GetKonta_urzednikow(int id)
        {
            var konta_urzednikow = await _context.Konta_urzednikow.FindAsync(id);

            if (konta_urzednikow == null)
            {
                return NotFound();
            }

            return konta_urzednikow;
        }

        // PUT: api/Konta_urzednikow/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutKonta_urzednikow(int id, Konta_urzednikow konta_urzednikow)
        {
            if (id != konta_urzednikow.id)
            {
                return BadRequest();
            }

            _context.Entry(konta_urzednikow).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Konta_urzednikowExists(id))
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

        // POST: api/Konta_urzednikow
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Konta_urzednikow>> PostKonta_urzednikow(Konta_urzednikow konta_urzednikow)
        {
            _context.Konta_urzednikow.Add(konta_urzednikow);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetKonta_urzednikow", new { id = konta_urzednikow.id }, konta_urzednikow);
        }

        // DELETE: api/Konta_urzednikow/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteKonta_urzednikow(int id)
        {
            var konta_urzednikow = await _context.Konta_urzednikow.FindAsync(id);
            if (konta_urzednikow == null)
            {
                return NotFound();
            }

            _context.Konta_urzednikow.Remove(konta_urzednikow);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool Konta_urzednikowExists(int id)
        {
            return _context.Konta_urzednikow.Any(e => e.id == id);
        }
    }
}
