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
    public class Akty_rozwoduController : ControllerBase
    {
        private readonly UrzadDBContext _context;

        public Akty_rozwoduController(UrzadDBContext context)
        {
            _context = context;
        }

        // GET: api/Akty_rozwodu
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Akty_rozwodu>>> GetAkty_rozwodu()
        {
            return await _context.Akty_rozwodu.ToListAsync();
        }

        // GET: api/Akty_rozwodu/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Akty_rozwodu>> GetAkty_rozwodu(int id)
        {
            var akty_rozwodu = await _context.Akty_rozwodu.FindAsync(id);

            if (akty_rozwodu == null)
            {
                return NotFound();
            }

            return akty_rozwodu;
        }

        // PUT: api/Akty_rozwodu/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAkty_rozwodu(int id, Akty_rozwodu akty_rozwodu)
        {
            if (id != akty_rozwodu.id)
            {
                return BadRequest();
            }

            _context.Entry(akty_rozwodu).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Akty_rozwoduExists(id))
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

        // POST: api/Akty_rozwodu
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Akty_rozwodu>> PostAkty_rozwodu(Akty_rozwodu akty_rozwodu)
        {
            _context.Akty_rozwodu.Add(akty_rozwodu);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAkty_rozwodu", new { id = akty_rozwodu.id }, akty_rozwodu);
        }

        // DELETE: api/Akty_rozwodu/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAkty_rozwodu(int id)
        {
            var akty_rozwodu = await _context.Akty_rozwodu.FindAsync(id);
            if (akty_rozwodu == null)
            {
                return NotFound();
            }

            _context.Akty_rozwodu.Remove(akty_rozwodu);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool Akty_rozwoduExists(int id)
        {
            return _context.Akty_rozwodu.Any(e => e.id == id);
        }
    }
}
