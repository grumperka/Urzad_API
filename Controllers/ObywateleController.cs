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
    public class ObywateleController : ControllerBase
    {
        private readonly UrzadDBContext _context;

        public ObywateleController(UrzadDBContext context)
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

        private List<int> getMarriedIdK(UrzadDBContext context) {
            List<int> obywateleSlubList = context.Akty_slubow.Select(s => s.id_malzonka).ToList();
            List<int> obywateleRozwodList = context.Akty_rozwodu.Select(s => s.id_rozwodnika).ToList();
            obywateleSlubList.RemoveAll(r => obywateleRozwodList.Contains(r));
            return obywateleSlubList;
        }

        private List<int> getMarriedIdM(UrzadDBContext context)
        {
            List<int> obywateleSlubList = context.Akty_slubow.Select(s => s.id_malzonki).ToList();
            List<int> obywateleRozwodList = context.Akty_rozwodu.Select(s => s.id_rozwodniczki).ToList();
            obywateleSlubList.RemoveAll(r => obywateleRozwodList.Contains(r));
            return obywateleSlubList;
        }

        // GET: api/Obywatele
        [HttpGet("getObywatelM")]
        public async Task<ActionResult<IEnumerable<Obywatele>>> GetObywatelM()
        {
            string header = _context.getAuthorizationHeader(HttpContext);
            var context = getContext(header);

            List<int> obywateleSlubList = getMarriedIdK(context);

            List<Obywatele> results = getAliveCitizensFromListToObywateleList(obywateleSlubList, context);

            return results;
        }

        [HttpGet("getObywatelK")]
        public async Task<ActionResult<IEnumerable<Obywatele>>> GetObywatelK()
        {
            string header = _context.getAuthorizationHeader(HttpContext);
            var context = getContext(header);

            List<int> obywateleSlubList = getMarriedIdM(context);

            List<Obywatele> results = getAliveCitizensFromListToObywateleList(obywateleSlubList, context);

            return results;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Obywatele>>> GetObywatele()
        {
            string header = _context.getAuthorizationHeader(HttpContext);
            var context = getContext(header);
            return await context.Obywatele.ToListAsync();
        }

        private List<Obywatele> getAliveCitizens(UrzadDBContext context) {

            List<Obywatele> listaObywateli = context.Obywatele.ToList();
            List<int> listaZgonow = context.Akty_zgonu.Select(s => s.id_obywatela).ToList();
            listaObywateli.RemoveAll(r => listaZgonow.Contains(r.id));
            return listaObywateli;
        }

        private List<Obywatele> getAliveCitizensFromListToObywateleList(List<int> citizensId, UrzadDBContext context)
        {
            List<int> listaZgonow = context.Akty_zgonu.Select(s => s.id_obywatela).ToList();
            citizensId.RemoveAll(r => listaZgonow.Contains(r));

            List<Obywatele> results = new List<Obywatele>();
            citizensId.ForEach(o =>
            {
                results.Add(context.Obywatele.Find(o));
            }
            );

            return results;
        }

        [HttpGet("getAlive")]
        public async Task<ActionResult<IEnumerable<Obywatele>>> getAlive()
        {
            string header = _context.getAuthorizationHeader(HttpContext);
            var context = getContext(header);
            return getAliveCitizens(context);
        }

        [HttpGet("getAliveWithoutAktUrodzenia")]
        public async Task<ActionResult<IEnumerable<Obywatele>>> getAlivewithoutAktUrodzenia()
        {
            string header = _context.getAuthorizationHeader(HttpContext);
            var context = getContext(header);
            List<Obywatele> result = getAliveCitizens(context);
            List<int> listaAktowUrodzenia = context.Akty_urodzenia.Select(s => s.id_obywatela).ToList();
            result.RemoveAll(r => listaAktowUrodzenia.Contains(r.id));
            return result;
        }

        [HttpGet("getSingle")]
        public async Task<ActionResult<IEnumerable<Obywatele>>> getSingle()
        {
            string header = _context.getAuthorizationHeader(HttpContext);
            var context = getContext(header);
            List<Obywatele> alive = getAliveCitizens(context);
            List<int> marriedK = getMarriedIdK(context);
            List<int> marriedM = getMarriedIdM(context);
            alive.RemoveAll(r => marriedK.Contains(r.id));
            alive.RemoveAll(r => marriedM.Contains(r.id));

            return alive;
        }

        // GET: api/Obywatele/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Obywatele>> GetObywatele(int id)
        {
            string header = _context.getAuthorizationHeader(HttpContext);
            var context = getContext(header);
            var obywatele = await context.Obywatele.FindAsync(id);

            if (obywatele == null)
            {
                return NotFound();
            }

            return obywatele;
        }

        [HttpGet("getRozwodniczki/{id_rozwodnika}")]
        public async Task<ActionResult<IEnumerable<Obywatele>>> getRozwodniczki(int id_rozwodnika) {
            string header = _context.getAuthorizationHeader(HttpContext);
            var context = getContext(header);

            List<int> id_malzonkiList = context.Akty_slubow.Where(w => w.id_malzonka == id_rozwodnika).Select(s => s.id_malzonki).ToList();
            List<Obywatele> malzonkiList = new List<Obywatele>();

            if(id_malzonkiList.Count() != 0) { 
            foreach(int id in id_malzonkiList)
            {
                bool ifDivorced = context.Akty_rozwodu.Where(w => w.id_rozwodnika == id_rozwodnika && w.id_rozwodniczki == id).Any();
                    if (ifDivorced)//szukamy malzonki!
                    {
                        id_malzonkiList.Remove(id);
                    }
                    else {
                        malzonkiList.Add(context.Obywatele.Find(id));
                    }
            }
            }

            return malzonkiList;
        }

        [HttpGet("getRozwodnikow/{id_rozwodniczki}")]
        public async Task<ActionResult<IEnumerable<Obywatele>>> getRozwodnikow(int id_rozwodniczki)
        {
            string header = _context.getAuthorizationHeader(HttpContext);
            var context = getContext(header);

            List<int> id_malzonkowieList = context.Akty_slubow.Where(w => w.id_malzonki == id_rozwodniczki).Select(s => s.id_malzonka).ToList();
            List<Obywatele> malzonkowieList = new List<Obywatele>();

            if (id_malzonkowieList.Count() != 0)
            {
                foreach (int id in id_malzonkowieList)
                {
                    bool ifDivorced = context.Akty_rozwodu.Where(w => w.id_rozwodniczki == id_rozwodniczki && w.id_rozwodnika == id).Any();
                    if (ifDivorced)//szukamy malzonka!
                    {
                        id_malzonkowieList.Remove(id);
                    }
                    else
                    {
                        malzonkowieList.Add(context.Obywatele.Find(id));
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

            string header = _context.getAuthorizationHeader(HttpContext);
            UrzadDBContext context = getContext(header);

            try
            {
                Obywatele obywateleOld = context.Obywatele.Find(id);
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
            catch (SqlException ex)
            {
                return Forbid(); //i tak nie zwraca :(
            }

            try
            {
                await context.SaveChangesAsync();
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
            string header = _context.getAuthorizationHeader(HttpContext);
            var context = getContext(header);

            obywatele.id = context.Obywatele.ToList().Last().id + 1;
            context.Obywatele.Add(obywatele);
            await context.SaveChangesAsync();

            return obywatele;
        }

        // DELETE: api/Obywatele/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteObywatele(int id)
        {
            string header = _context.getAuthorizationHeader(HttpContext);
            var context = getContext(header);

            var obywatele = await context.Obywatele.FindAsync(id);
            if (obywatele == null)
            {
                return NotFound();
            }

            context.Obywatele.Remove(obywatele);
            await context.SaveChangesAsync();

            return NoContent();
        }

        private bool ObywateleExists(int id)
        {
            string header = _context.getAuthorizationHeader(HttpContext);
            var context = getContext(header);
            return context.Obywatele.Any(e => e.id == id);
        }
    }
}
