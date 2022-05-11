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
using Microsoft.Data.SqlClient;

namespace KSiwiak_Urzad_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KierownicyController : ControllerBase
    {
        private UrzadDBContext _context;
        private int index;

        public KierownicyController(UrzadDBContext context)
        {
            _context = context;
            index = _context.Kierownicy.ToList().Last().id;
        }

        // GET: api/Kierownicy
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Kierownicy>>> GetKierownicy()
        {
            var contextOptions = new DbContextOptionsBuilder<UrzadDBContext>()
            .UseSqlServer("Data Source = GRUMPERKA\\SIWIAK; Initial Catalog = Urzedy; Integrated Security = True; MultipleActiveResultSets = True;User Id = hansKarlsson;Password=hans2002;")
            .Options;
            this._context = new UrzadDBContext(contextOptions);
            return await _context.Kierownicy.ToListAsync();
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<IEnumerable<Kierownicy_Urzad>>> GetKierownicy_Urzad()
        {
                var kierownicyList =  await _context.Kierownicy.Where(w => w.czy_zastepca_T_N == 0).ToListAsync();
                var UrzedyList = await _context.Urzedy.ToListAsync();

                List<Kierownicy_Urzad> kierownicy_Urzad_list = new List<Kierownicy_Urzad>();

                foreach (Kierownicy k in kierownicyList) {
                    string nazwaUrzedu = UrzedyList.Where(w => w.id == k.urzad_id).FirstOrDefault().nazwa_urzedu;

                    if (nazwaUrzedu != null) { 
                    kierownicy_Urzad_list.Add(new Kierownicy_Urzad { id = k.id, imie = k.imie, nazwisko = k.nazwisko, urzad_id = k.urzad_id, nazwa_urzedu = nazwaUrzedu, czy_zastepca_T_N = k.czy_zastepca_T_N, login = k.login } );
                    }
                }

                return kierownicy_Urzad_list;

        }

        // GET: api/Kierownicy/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Kierownicy>> GetKierownicy(int id)
        {
            var kierownicy = await _context.Kierownicy.FindAsync(id);

            if (kierownicy == null)
            {
                return NotFound();
            }

            return kierownicy;
        }

        // PUT: api/Kierownicy/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutKierownicy(int id, Kierownicy kierownicy)
        {
            if (id != kierownicy.id)
            {
                return BadRequest();
            }

            //_context.Entry(kierownicy).State = EntityState.Modified;

            try
            {
                Kierownicy kierownicyOld = _context.Kierownicy.Find(id);
                kierownicyOld.imie = kierownicy.imie;
                kierownicyOld.nazwisko = kierownicy.nazwisko;
                kierownicyOld.urzad_id = kierownicy.urzad_id;
                kierownicyOld.czy_zastepca_T_N = kierownicy.czy_zastepca_T_N;
                kierownicyOld.login = kierownicy.imie.ToLower() + kierownicy.nazwisko + "_" + id + "@poczta.pl";
            }
            catch (ArgumentNullException ex)
            {
                return NotFound();
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!KierownicyExists(id))
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

        // POST: api/Kierownicy
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Kierownicy>> PostKierownicy(Kierownicy kierownicy)
        {
            this.index += 1;
            kierownicy.id = this.index;
            kierownicy.login = kierownicy.imie.ToLower() + kierownicy.nazwisko + "_" + kierownicy.id + "@poczta.pl";
            _context.Kierownicy.Add(kierownicy);
            await _context.SaveChangesAsync();

            return kierownicy;
        }

        // DELETE: api/Kierownicy/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteKierownicy(int id)
        {
            var kierownicy = await _context.Kierownicy.FindAsync(id);
            if (kierownicy == null)
            {
                return NotFound();
            }

            _context.Kierownicy.Remove(kierownicy);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool KierownicyExists(int id)
        {
            return _context.Kierownicy.Any(e => e.id == id);
        }
    }
}
