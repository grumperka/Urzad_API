﻿#nullable disable
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
    public class Akty_zgonuController : ControllerBase
    {
        private readonly UrzadDBContext _context;
        private int index;

        public Akty_zgonuController(UrzadDBContext context)
        {
            _context = context;
            index = _context.Akty_zgonu.ToList().Last().id;
        }

        // GET: api/Akty_zgonu
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Akty_zgonu>>> GetAkty_zgonu()
        {
            return await _context.Akty_zgonu.ToListAsync();
        }

        // GET: api/Akty_zgonu/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Akty_zgonu>> GetAkty_zgonu(int id)
        {
            var akty_zgonu = await _context.Akty_zgonu.FindAsync(id);

            if (akty_zgonu == null)
            {
                return NotFound();
            }

            return akty_zgonu;
        }

        // PUT: api/Akty_zgonu/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutAkty_zgonu(int id, Akty_zgonu akty_zgonu)
        //{
        //    if (id != akty_zgonu.id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(akty_zgonu).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!Akty_zgonuExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        // POST: api/Akty_zgonu
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Akty_zgonu>> PostAkty_zgonu(Akty_zgonu akty_zgonu)
        {
            this.index += 1;
            akty_zgonu.id = this.index;
            akty_zgonu.id_urzedu = _context.Urzednicy.Find(akty_zgonu.id_urzednika).urzad_id;
            _context.Akty_zgonu.Add(akty_zgonu);
            await _context.SaveChangesAsync();

            return akty_zgonu;
        }

        // DELETE: api/Akty_zgonu/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAkty_zgonu(int id)
        {
            var akty_zgonu = await _context.Akty_zgonu.FindAsync(id);
            if (akty_zgonu == null)
            {
                return NotFound();
            }

            _context.Akty_zgonu.Remove(akty_zgonu);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool Akty_zgonuExists(int id)
        {
            return _context.Akty_zgonu.Any(e => e.id == id);
        }
    }
}
