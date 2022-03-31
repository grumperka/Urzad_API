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
    public class ObywateleController : ControllerBase
    {
        private readonly UrzadDBContext _context;

        public ObywateleController(UrzadDBContext context)
        {
            _context = context;
        }

        // GET: api/Obywatele
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Obywatele>>> GetObywatele()
        {
            return await _context.Obywatele.ToListAsync();
        }

        // GET: api/Obywatele/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Obywatele>> GetObywatele(int id)
        {
            var obywatele = await _context.Obywatele.FindAsync(id);

            if (obywatele == null)
            {
                return NotFound();
            }

            return obywatele;
        }

        // PUT: api/Obywatele/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutObywatele(int id, Obywatele obywatele)
        {
            if (id != obywatele.id)
            {
                return BadRequest();
            }

            _context.Entry(obywatele).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ObywateleExists(id))
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

        // POST: api/Obywatele
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Obywatele>> PostObywatele(Obywatele obywatele)
        {
            _context.Obywatele.Add(obywatele);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetObywatele", new { id = obywatele.id }, obywatele);
        }

        // DELETE: api/Obywatele/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteObywatele(int id)
        {
            var obywatele = await _context.Obywatele.FindAsync(id);
            if (obywatele == null)
            {
                return NotFound();
            }

            _context.Obywatele.Remove(obywatele);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ObywateleExists(int id)
        {
            return _context.Obywatele.Any(e => e.id == id);
        }
    }
}
