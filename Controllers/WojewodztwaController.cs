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
    public class WojewodztwaController : ControllerBase
    {
        private readonly UrzadDBContext _context;

        public WojewodztwaController(UrzadDBContext context)
        {
            _context = context;
        }

        // GET: api/Wojewodztwa
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Wojewodztwa>>> GetWojewodztwa()
        {
            return await _context.Wojewodztwa.ToListAsync();
        }

        // GET: api/Wojewodztwa/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Wojewodztwa>> GetWojewodztwa(int id)
        {
            var wojewodztwa = await _context.Wojewodztwa.FindAsync(id);

            if (wojewodztwa == null)
            {
                return NotFound();
            }

            return wojewodztwa;
        }
    
        private bool WojewodztwaExists(int id)
        {
            return _context.Wojewodztwa.Any(e => e.id == id);
        }
    }
}
