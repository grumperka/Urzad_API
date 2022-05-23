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
using Microsoft.Data.SqlClient;

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

        // GET: api/Akty_rozwodu
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Akty_rozwodu>>> GetAkty_rozwodu()
        {
            string header = _context.getAuthorizationHeader(HttpContext);
            var context = getContext(header);
            return await context.Akty_rozwodu.ToListAsync();
        }

        // GET: api/Akty_rozwodu/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Akty_rozwodu>> GetAkty_rozwodu(int id)
        {
            string header = _context.getAuthorizationHeader(HttpContext);
            var context = getContext(header);

            var akty_rozwodu = await context.Akty_rozwodu.FindAsync(id);

            if (akty_rozwodu == null)
            {
               return NotFound();
            }

            return akty_rozwodu;
        }

        [HttpGet("getAkt_rozwoduFromUser/{id}")]
        public async Task<ActionResult<List<Akty_rozwodu>>> getAkt_rozwoduFromUser(int id)
        {
            string header = _context.getAuthorizationHeader(HttpContext);
            var context = getContext(header);

            var akty_rozwodu = await context.Akty_rozwodu.Where(w => w.id_rozwodnika == id || w.id_rozwodniczki == id).ToListAsync();

            if (akty_rozwodu == null)
            {
                return NotFound();
            }

            return akty_rozwodu;
        }

        [HttpGet("getAkt_rozwoduFromUrzednik/{id}")]
        public async Task<ActionResult<List<Akty_rozwodu>>> GetAkt_rozwoduFromUrzednik(int id)
        {
            string header = _context.getAuthorizationHeader(HttpContext);
            var context = getContext(header);

            var akty_rozwodu = await context.Akty_rozwodu.Where(w => w.id_urzednika == id).ToListAsync();

            if (akty_rozwodu == null)
            {
                return NotFound();
            }

            return akty_rozwodu;
        }

        [HttpGet("getAkt_rozwoduFromUrzad/{id}")]
        public async Task<ActionResult<List<Akty_rozwodu>>> GetAkt_rozwoduFromUrzad(int id)
        {
            string header = _context.getAuthorizationHeader(HttpContext);
            var context = getContext(header);

            int urzadID = context.Kierownicy.Find(id).urzad_id;
            if (urzadID != null)
            {
                var akty_rozwodu = await context.Akty_rozwodu.Where(w => w.id_urzedu == urzadID).ToListAsync();

                if (akty_rozwodu == null)
                {
                    return NotFound();
                }
                return akty_rozwodu;
            }

            return NotFound();
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

            string header = _context.getAuthorizationHeader(HttpContext);
            UrzadDBContext context = getContext(header);

            try
            {
                Akty_rozwodu akty_RozwoduOld = context.Akty_rozwodu.Find(id);   /////
                akty_RozwoduOld.id_powodu_glownego = akty_rozwodu.id_powodu_glownego;
                akty_RozwoduOld.z_orzekaniem_winy_T_N = akty_rozwodu.z_orzekaniem_winy_T_N;
                akty_RozwoduOld.czy_wylacznie_T_N = akty_rozwodu.czy_wylacznie_T_N;
            }
            catch (ArgumentNullException ex)
            {
                return NotFound();
            }
            catch (SqlException ex) {
                return Forbid(); //i tak nie zwraca :(
            }

            try
            {
                await context.SaveChangesAsync();
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
            string header = _context.getAuthorizationHeader(HttpContext);
            var context = getContext(header);

            akty_rozwodu.id = context.Akty_rozwodu.ToList().Last().id + 1;
            akty_rozwodu.id_urzedu = context.Urzednicy.Find(akty_rozwodu.id_urzednika).urzad_id;
            context.Akty_rozwodu.Add(akty_rozwodu);
            await context.SaveChangesAsync();

            return akty_rozwodu;

        }

        // DELETE: api/Akty_rozwodu/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAkty_rozwodu(int id)
        {
            string header = _context.getAuthorizationHeader(HttpContext);
            var context = getContext(header);

            var akty_rozwodu = await context.Akty_rozwodu.FindAsync(id);
                    if (akty_rozwodu == null)
                    {
                        return NotFound();
                    }

            context.Akty_rozwodu.Remove(akty_rozwodu);
            await context.SaveChangesAsync();

            return NoContent();
        }

        private bool Akty_rozwoduExists(int id)
        {
            string header = _context.getAuthorizationHeader(HttpContext);
            var context = getContext(header);
            return _context.Akty_rozwodu.Any(e => e.id == id);
        }
    }
}
