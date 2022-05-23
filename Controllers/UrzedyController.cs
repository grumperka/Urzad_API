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
using KSiwiak_Urzad_API.Models;

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

        // GET: api/Urzedy
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Urzad_Woj>>> GetUrzedy()
        {
            string header = _context.getAuthorizationHeader(HttpContext);
            var context = getContext(header);

            List<Urzad_Woj> results = new List<Urzad_Woj>();
            var wojList = context.Wojewodztwa.ToList();
            foreach (Urzedy urzedy in context.Urzedy.ToList()) {
                Urzad_Woj urzad = new Urzad_Woj { id = urzedy.id, nazwa_urzedu = urzedy.nazwa_urzedu, 
                                                wojewodztwo_id = urzedy.wojewodztwo_id, 
                                                nazwa_wojewodztwa = wojList.Where(w => w.id == urzedy.wojewodztwo_id).FirstOrDefault().nazwa};

                results.Add(urzad);
            }
            
            return results;
        }

        // GET: api/Urzedy/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Urzad_Woj>> GetUrzedy(int id)
        {
            string header = _context.getAuthorizationHeader(HttpContext);
            var context = getContext(header);

            var urzedy = await context.Urzedy.FindAsync(id);

            if (urzedy == null)
            {
                return NotFound();
            }

            string nazwaWoj = context.Wojewodztwa.Where(w => w.id == urzedy.wojewodztwo_id).FirstOrDefault().nazwa;

            return new Urzad_Woj { id = urzedy.id, nazwa_urzedu = urzedy.nazwa_urzedu, nazwa_wojewodztwa = nazwaWoj, wojewodztwo_id = urzedy.wojewodztwo_id };
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

            string header = _context.getAuthorizationHeader(HttpContext);
            var context = getContext(header);

            try
            {
                Urzedy urzadOld = context.Urzedy.Find(id);
                urzadOld.nazwa_urzedu = urzedy.nazwa_urzedu;
                urzadOld.wojewodztwo_id = urzedy.wojewodztwo_id;
            }
            catch (ArgumentNullException ex) {
                return NotFound();
            }


            try
            {
                await context.SaveChangesAsync();
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
        public async Task<ActionResult<Urzad_Woj>> PostUrzedy(Urzedy urzedy)
        {
            string header = _context.getAuthorizationHeader(HttpContext);
            var context = getContext(header);

            int index = context.Urzedy.ToList().Last().id + 1;
            Urzedy newUrzad = new Urzedy { id = index, nazwa_urzedu = urzedy.nazwa_urzedu, wojewodztwo_id = urzedy.wojewodztwo_id };
            context.Urzedy.Add(newUrzad);
            await context.SaveChangesAsync();

            string nazwa_woj = context.Wojewodztwa.Where(w => w.id == newUrzad.wojewodztwo_id).FirstOrDefault().nazwa;
            
            return new Urzad_Woj { id = newUrzad.id, nazwa_urzedu = newUrzad.nazwa_urzedu, nazwa_wojewodztwa = nazwa_woj, wojewodztwo_id = newUrzad.wojewodztwo_id };
        }

        // DELETE: api/Urzedy/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUrzedy(int id)
        {
            string header = _context.getAuthorizationHeader(HttpContext);
            var context = getContext(header);

            var urzedy = await context.Urzedy.FindAsync(id);
            if (urzedy == null)
            {
                return NotFound();
            }

            context.Urzedy.Remove(urzedy);
            await context.SaveChangesAsync();

            return Ok();
        }

        private bool UrzedyExists(int id)
        {
            string header = _context.getAuthorizationHeader(HttpContext);
            var context = getContext(header);
            return context.Urzedy.Any(e => e.id == id);
        }
    }
}
