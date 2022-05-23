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

        [ApiExplorerSettings(IgnoreApi = true)]
        public UrzadDBContext getContext(string header)
        {

            if (!header.Equals(""))
            {
                var contextOptions = new DbContextOptionsBuilder<UrzadDBContext>()
                .UseSqlServer("Data Source = GRUMPERKA\\SIWIAK; Initial Catalog = Urzedy; Integrated Security = False;" + header).Options;
                return new UrzadDBContext(contextOptions);
            }
            else
            {
                return this._context;
            }
        }

            // GET: api/Akty_slubow
            [HttpGet]
        public async Task<ActionResult<IEnumerable<Akty_slubow>>> GetAkty_slubow()
        {
            string header = _context.getAuthorizationHeader(HttpContext);
            var context = getContext(header);
            return await context.Akty_slubow.ToListAsync();
        }

        // GET: api/Akty_slubow/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Akty_slubow>> GetAkty_slubow(int id)
        {
            string header = _context.getAuthorizationHeader(HttpContext);
            var context = getContext(header);

            var akty_slubow = await context.Akty_slubow.FindAsync(id);

            if (akty_slubow == null)
            {
                return NotFound();
            }

            return akty_slubow;
        }

        [HttpGet("getAkt_slubuFromUser/{id}")]
        public async Task<ActionResult<List<Akty_slubow>>> GetAkt_slubuFromUser(int id)
        {
            string header = _context.getAuthorizationHeader(HttpContext);
            var context = getContext(header);

            var akty_slubow = await context.Akty_slubow.Where(w => w.id_malzonka == id || w.id_malzonki == id).ToListAsync();

            if (akty_slubow == null)
            {
                return NotFound();
            }

            return akty_slubow;
        }

        [HttpGet("getAkt_slubuFromUrzednik/{id}")]
        public async Task<ActionResult<List<Akty_slubow>>> GetAkt_slubuFromUrzednik(int id)
        {
            string header = _context.getAuthorizationHeader(HttpContext);
            var context = getContext(header);

            var akty_slubow = await context.Akty_slubow.Where(w => w.id_urzednika == id).ToListAsync();

            if (akty_slubow == null)
            {
                return NotFound();
            }

            return akty_slubow;
        }

        [HttpGet("getAkt_slubuFromUrzad/{id}")]
        public async Task<ActionResult<List<Akty_slubow>>> GetAkt_slubuFromUrzad(int id)
        {
            string header = _context.getAuthorizationHeader(HttpContext);
            var context = getContext(header);

            int urzadID = context.Kierownicy.Find(id).urzad_id;
            if (urzadID != null)
            {
                var akty_slubow = await context.Akty_slubow.Where(w => w.id_urzedu == urzadID).ToListAsync();

                if (akty_slubow == null)
                {
                    return NotFound();
                }

                return akty_slubow;
            }
            return NotFound();
        }

        // POST: api/Akty_slubow
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Akty_slubow>> PostAkty_slubow(Akty_slubow akty_slubow)
        {
            string header = _context.getAuthorizationHeader(HttpContext);
            var context = getContext(header);

            akty_slubow.id = context.Akty_slubow.ToList().Last().id + 1;
            akty_slubow.id_urzedu = context.Urzednicy.Find(akty_slubow.id_urzednika).urzad_id;
            context.Akty_slubow.Add(akty_slubow);
            await context.SaveChangesAsync();

            return akty_slubow;

        }

        // DELETE: api/Akty_slubow/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAkty_slubow(int id)
        {
            string header = _context.getAuthorizationHeader(HttpContext);
            var context = getContext(header);

            var akty_slubow = await context.Akty_slubow.FindAsync(id);
            if (akty_slubow == null)
            {
                return NotFound();
            }

            context.Akty_slubow.Remove(akty_slubow);
            await context.SaveChangesAsync();

            return NoContent();
        }

        private bool Akty_slubowExists(int id)
        {
            string header = _context.getAuthorizationHeader(HttpContext);
            var context = getContext(header);
            return context.Akty_slubow.Any(e => e.id == id);
        }
    }
}
