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
    public class Powody_rozwoduController : ControllerBase
    {
        private readonly UrzadDBContext _context;

        public Powody_rozwoduController(UrzadDBContext context)
        {
            _context = context;
        }

        // GET: api/Powody_rozwodu
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Powody_rozwodu>>> GetPowody_rozwodu()
        {
            return await _context.Powody_rozwodu.ToListAsync();
        }

        // GET: api/Powody_rozwodu/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Powody_rozwodu>> GetPowody_rozwodu(int id)
        {
            var powody_rozwodu = await _context.Powody_rozwodu.FindAsync(id);

            if (powody_rozwodu == null)
            {
                return NotFound();
            }

            return powody_rozwodu;
        }


        private bool Powody_rozwoduExists(int id)
        {
            return _context.Powody_rozwodu.Any(e => e.id == id);
        }
    }
}
