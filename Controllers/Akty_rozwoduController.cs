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
        private int index;

        public Akty_rozwoduController(UrzadDBContext context)
        {
            _context = context;
            index = _context.Akty_rozwodu.ToList().Last().id;
        }

        // GET: api/Akty_rozwodu
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Akty_rozwodu>>> GetAkty_rozwodu()
        {
            //if (_context.isSessionOpen(HttpContext))
            //{

            //    string rola = _context.getRola(HttpContext);

            //    if (rola.Equals("urzednik") || rola.Equals("kierownik"))
            //    {
                    return await _context.Akty_rozwodu.ToListAsync();
        //        }
        //        else return Forbid();
        //    }
        //    else return Forbid();
        }

        // GET: api/Akty_rozwodu/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Akty_rozwodu>> GetAkty_rozwodu(int id)
        {
            //if (_context.isSessionOpen(HttpContext))
            //{

            //    string rola = _context.getRola(HttpContext);

            //    if (rola.Equals("urzednik") || rola.Equals("kierownik"))
            //    {
                    var akty_rozwodu = await _context.Akty_rozwodu.FindAsync(id);

                    if (akty_rozwodu == null)
                    {
                        return NotFound();
                    }

                    return akty_rozwodu;
            //    }
            //    else if (rola.Equals("obywatel")) {
            //        var akty_rozwodu = await _context.Akty_rozwodu.FindAsync(id);

            //        if (akty_rozwodu == null || akty_rozwodu.id_rozwodnika != _context.getUserId(HttpContext) || akty_rozwodu.id_rozwodniczki != _context.getUserId(HttpContext))
            //        {
            //            return NotFound();
            //        }

            //        return akty_rozwodu;
            //    }
            //    else return Forbid();
            //}
            //else return Forbid();
        }

        [HttpGet("getAkt_rozwoduFromUser/{id}")]
        public async Task<ActionResult<List<Akty_rozwodu>>> getAkt_rozwoduFromUser(int id)
        {
            var akty_rozwodu = await _context.Akty_rozwodu.Where(w => w.id_rozwodnika == id || w.id_rozwodniczki == id).ToListAsync();

            if (akty_rozwodu == null)
            {
                return NotFound();
            }

            return akty_rozwodu;
        }

        [HttpGet("getAkt_rozwoduFromUrzednik/{id}")]
        public async Task<ActionResult<List<Akty_rozwodu>>> GetAkt_rozwoduFromUrzednik(int id)
        {
            var akty_rozwodu = await _context.Akty_rozwodu.Where(w => w.id_urzednika == id).ToListAsync();

            if (akty_rozwodu == null)
            {
                return NotFound();
            }

            return akty_rozwodu;
        }

        [HttpGet("getAkt_rozwoduFromUrzad/{id}")]
        public async Task<ActionResult<List<Akty_rozwodu>>> GetAkt_rozwoduFromUrzad(int id)
        {
            int urzadID = _context.Kierownicy.Find(id).urzad_id;
            if (urzadID != null)
            {
                var akty_rozwodu = await _context.Akty_rozwodu.Where(w => w.id_urzedu == urzadID).ToListAsync();

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
        //    if (_context.isSessionOpen(HttpContext))
        //    {
        //        string rola = _context.getRola(HttpContext);

        //        if (rola.Equals("urzednik") || rola.Equals("kierownik"))
        //        {
                    if (id != akty_rozwodu.id)
                        {
                            return BadRequest();
                        }

                        //_context.Entry(akty_rozwodu).State = EntityState.Modified;

                        try
                        {
                            Akty_rozwodu akty_RozwoduOld = _context.Akty_rozwodu.Find(id);
                            akty_RozwoduOld.id_powodu_glownego = akty_rozwodu.id_powodu_glownego;
                            akty_RozwoduOld.z_orzekaniem_winy_T_N = akty_rozwodu.z_orzekaniem_winy_T_N;
                            akty_RozwoduOld.czy_wylacznie_T_N = akty_rozwodu.czy_wylacznie_T_N;
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
            //    }
            //    else return Forbid();
            //}
            //else return Forbid();
        }

        // POST: api/Akty_rozwodu
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Akty_rozwodu>> PostAkty_rozwodu(Akty_rozwodu akty_rozwodu)
        {
            //if (_context.isSessionOpen(HttpContext))
            //{
            //    string rola = _context.getRola(HttpContext);

            //    if (rola.Equals("urzednik") || rola.Equals("kierownik"))
            //    {
                    this.index += 1;
                    akty_rozwodu.id = this.index;
                    akty_rozwodu.id_urzedu = _context.Urzednicy.Find(akty_rozwodu.id_urzednika).urzad_id;
                    _context.Akty_rozwodu.Add(akty_rozwodu);
                    await _context.SaveChangesAsync();

                    return akty_rozwodu;
            //    }
            //    else return Forbid();
            //}
            //else return Forbid();
        }

        // DELETE: api/Akty_rozwodu/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAkty_rozwodu(int id)
        {
            //if (_context.isSessionOpen(HttpContext))
            //{

            //    string rola = _context.getRola(HttpContext);

            //    if (rola.Equals("urzednik") || rola.Equals("kierownik"))
            //    {
                    var akty_rozwodu = await _context.Akty_rozwodu.FindAsync(id);
                    if (akty_rozwodu == null)
                    {
                        return NotFound();
                    }

                    _context.Akty_rozwodu.Remove(akty_rozwodu);
                    await _context.SaveChangesAsync();

                    return NoContent();
            //    }
            //    else return Forbid();
            //}
            //else return Forbid();
        }

        private bool Akty_rozwoduExists(int id)
        {
            return _context.Akty_rozwodu.Any(e => e.id == id);
        }
    }
}
