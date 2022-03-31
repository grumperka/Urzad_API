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
    public class UrzedyController : ControllerBase
    {
        private readonly UrzadDBContext _context;

        public UrzedyController(UrzadDBContext context)
        {
            _context = context;
        }

        // GET: api/Urzedy
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Urzedy>>> GetUrzedy()
        {
            return await _context.Urzedy.ToListAsync();
        }

        // GET: api/Urzedy/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Urzedy>> GetUrzedy(int id)
        {
            var urzedy = await _context.Urzedy.FindAsync(id);

            if (urzedy == null)
            {
                return NotFound();
            }

            return urzedy;
        }

        // PUT: api/Urzedy/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUrzedy(int id, Urzedy urzedy)
        {
            if (id != urzedy.id)
            {
                return BadRequest();
            }

            _context.Entry(urzedy).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UrzedyExists(id))
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

        // POST: api/Urzedy
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Urzedy>> PostUrzedy(Urzedy urzedy)
        {
            _context.Urzedy.Add(urzedy);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUrzedy", new { id = urzedy.id }, urzedy);
        }

        // DELETE: api/Urzedy/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUrzedy(int id)
        {
            var urzedy = await _context.Urzedy.FindAsync(id);
            if (urzedy == null)
            {
                return NotFound();
            }

            _context.Urzedy.Remove(urzedy);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UrzedyExists(int id)
        {
            return _context.Urzedy.Any(e => e.id == id);
        }
    }
}
