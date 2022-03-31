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

        public Akty_slubowController(UrzadDBContext context)
        {
            _context = context;
        }

        // GET: api/Akty_slubow
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Akty_slubow>>> GetAkty_slubow()
        {
            return await _context.Akty_slubow.ToListAsync();
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

        // PUT: api/Akty_slubow/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAkty_slubow(int id, Akty_slubow akty_slubow)
        {
            if (id != akty_slubow.id)
            {
                return BadRequest();
            }

            _context.Entry(akty_slubow).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Akty_slubowExists(id))
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

        // POST: api/Akty_slubow
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Akty_slubow>> PostAkty_slubow(Akty_slubow akty_slubow)
        {
            _context.Akty_slubow.Add(akty_slubow);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAkty_slubow", new { id = akty_slubow.id }, akty_slubow);
        }

        // DELETE: api/Akty_slubow/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAkty_slubow(int id)
        {
            var akty_slubow = await _context.Akty_slubow.FindAsync(id);
            if (akty_slubow == null)
            {
                return NotFound();
            }

            _context.Akty_slubow.Remove(akty_slubow);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool Akty_slubowExists(int id)
        {
            return _context.Akty_slubow.Any(e => e.id == id);
        }
    }
}
