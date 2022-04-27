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
        private int index;

        public ObywateleController(UrzadDBContext context)
        {
            _context = context;
            index = _context.Obywatele.ToList().Last().id;
        }

        private List<int> getMarriedIdK() {
            List<int> obywateleSlubList = _context.Akty_slubow.Select(s => s.id_malzonka).ToList();
            List<int> obywateleRozwodList = _context.Akty_rozwodu.Select(s => s.id_rozwodnika).ToList();
            obywateleSlubList.RemoveAll(r => obywateleRozwodList.Contains(r));
            return obywateleSlubList;
        }

        private List<int> getMarriedIdM()
        {
            List<int> obywateleSlubList = _context.Akty_slubow.Select(s => s.id_malzonki).ToList();
            List<int> obywateleRozwodList = _context.Akty_rozwodu.Select(s => s.id_rozwodniczki).ToList();
            obywateleSlubList.RemoveAll(r => obywateleRozwodList.Contains(r));
            return obywateleSlubList;
        }

        // GET: api/Obywatele
        [HttpGet("getObywatelM")]
        public async Task<ActionResult<IEnumerable<Obywatele>>> GetObywatelM()
        {
            List<int> obywateleSlubList = getMarriedIdK();

            List<Obywatele> results = getAliveCitizensFromListToObywateleList(obywateleSlubList);

            return results;
        }

        [HttpGet("getObywatelK")]
        public async Task<ActionResult<IEnumerable<Obywatele>>> GetObywatelK()
        {
            List<int> obywateleSlubList = getMarriedIdM();

            List<Obywatele> results = getAliveCitizensFromListToObywateleList(obywateleSlubList);

            return results;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Obywatele>>> GetObywatele()
        {
            return await _context.Obywatele.ToListAsync();
        }

        private List<Obywatele> getAliveCitizens() {
            List<Obywatele> listaObywateli = _context.Obywatele.ToList();
            List<int> listaZgonow = _context.Akty_zgonu.Select(s => s.id_obywatela).ToList();
            listaObywateli.RemoveAll(r => listaZgonow.Contains(r.id));
            return listaObywateli;
        }

        private List<Obywatele> getAliveCitizensFromListToObywateleList(List<int> citizensId)
        {
            List<int> listaZgonow = _context.Akty_zgonu.Select(s => s.id_obywatela).ToList();
            citizensId.RemoveAll(r => listaZgonow.Contains(r));

            List<Obywatele> results = new List<Obywatele>();
            citizensId.ForEach(o =>
            {
                results.Add(_context.Obywatele.Find(o));
            }
            );

            return results;
        }

        [HttpGet("getAlive")]
        public async Task<ActionResult<IEnumerable<Obywatele>>> getAlive()
        {
            return getAliveCitizens();
        }

        [HttpGet("getAliveWithoutAktUrodzenia")]
        public async Task<ActionResult<IEnumerable<Obywatele>>> getAlivewithoutAktUrodzenia()
        {
            List<Obywatele> result = getAliveCitizens();
            List<int> listaAktowUrodzenia = _context.Akty_urodzenia.Select(s => s.id_obywatela).ToList();
            result.RemoveAll(r => listaAktowUrodzenia.Contains(r.id));
            return result;
        }

        [HttpGet("getSingle")]
        public async Task<ActionResult<IEnumerable<Obywatele>>> getSingle()
        {
            List<Obywatele> alive = getAliveCitizens();
            List<int> marriedK = getMarriedIdK();
            List<int> marriedM = getMarriedIdM();
            alive.RemoveAll(r => marriedK.Contains(r.id));
            alive.RemoveAll(r => marriedM.Contains(r.id));

            return alive;
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

        [HttpGet("getRozwodniczki/{id_rozwodnika}")]
        public async Task<ActionResult<IEnumerable<Obywatele>>> getRozwodniczki(int id_rozwodnika) {
            List<int> id_malzonkiList = _context.Akty_slubow.Where(w => w.id_malzonka == id_rozwodnika).Select(s => s.id_malzonki).ToList();
            List<Obywatele> malzonkiList = new List<Obywatele>();

            if(id_malzonkiList.Count() != 0) { 
            foreach(int id in id_malzonkiList)
            {
                bool ifDivorced = _context.Akty_rozwodu.Where(w => w.id_rozwodnika == id_rozwodnika && w.id_rozwodniczki == id).Any();
                    if (ifDivorced)//szukamy malzonki!
                    {
                        id_malzonkiList.Remove(id);
                    }
                    else {
                        malzonkiList.Add(_context.Obywatele.Find(id));
                    }
            }
            }

            return malzonkiList;
        }

        [HttpGet("getRozwodnikow/{id_rozwodniczki}")]
        public async Task<ActionResult<IEnumerable<Obywatele>>> getRozwodnikow(int id_rozwodniczki)
        {
            List<int> id_malzonkowieList = _context.Akty_slubow.Where(w => w.id_malzonki == id_rozwodniczki).Select(s => s.id_malzonka).ToList();
            List<Obywatele> malzonkowieList = new List<Obywatele>();

            if (id_malzonkowieList.Count() != 0)
            {
                foreach (int id in id_malzonkowieList)
                {
                    bool ifDivorced = _context.Akty_rozwodu.Where(w => w.id_rozwodniczki == id_rozwodniczki && w.id_rozwodnika == id).Any();
                    if (ifDivorced)//szukamy malzonka!
                    {
                        id_malzonkowieList.Remove(id);
                    }
                    else
                    {
                        malzonkowieList.Add(_context.Obywatele.Find(id));
                    }
                }
            }

            return malzonkowieList;
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

            //_context.Entry(obywatele).State = EntityState.Modified;

            try
            {
                Obywatele obywateleOld = _context.Obywatele.Find(id);
                obywateleOld.imie = obywatele.imie;
                obywateleOld.nazwisko = obywatele.nazwisko;
                obywateleOld.nazwisko_rodowe = obywatele.nazwisko_rodowe;
                obywateleOld.plec = obywatele.plec;
                obywateleOld.pesel = obywatele.pesel;
                obywateleOld.wojewodztwo_id = obywatele.wojewodztwo_id;
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
            this.index += 1;
            obywatele.id = this.index;
            _context.Obywatele.Add(obywatele);
            await _context.SaveChangesAsync();

            return obywatele;
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
