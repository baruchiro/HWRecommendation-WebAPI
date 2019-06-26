using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HWWebApi.Models;
using Microsoft.AspNetCore.Hosting.Internal;
using Models;

namespace HWWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScansController : ControllerBase
    {
        private readonly HardwareContext _context;

        public ScansController(HardwareContext context)
        {
            _context = context;
        }

        // GET: api/Scans/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Scan>> GetScan(Guid id)
        {
            if (_context.Scans.Any(c => c.Id == id))
            {
                return Ok(await _context.Scans
                    .Include(s=>s.User)
                    .Include(s=>s.Computer)
                    .SingleAsync(c => c.Id == id));
            }

            return NotFound();
        }

        // POST: api/Scans
        [HttpPost]
        public async Task<ActionResult<Scan>> PostNewScanByComputer(Computer computer)
        {
            var scan = new Scan
            {
                Computer = computer,
                CreationDateTime = DateTime.Now
            };
            _context.Scans.Add(scan);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetScan", new { id = scan.Id }, scan);
        }
    }
}
