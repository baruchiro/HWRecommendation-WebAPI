using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HWWebApi.Models;

namespace HWWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestStringsController : ControllerBase
    {
        private readonly HardwareContext _context;

        public TestStringsController(HardwareContext context)
        {
            _context = context;
        }

        // GET: api/TestStrings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TestString>>> GetTestStrings()
        {
            return await _context.TestStrings.ToListAsync();
        }

        // GET: api/TestStrings/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TestString>> GetTestString(long id)
        {
            var testString = await _context.TestStrings.FindAsync(id);

            if (testString == null)
            {
                return NotFound();
            }

            return testString;
        }

        // PUT: api/TestStrings/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTestString(long id, TestString testString)
        {
            if (id != testString.Id)
            {
                return BadRequest();
            }

            _context.Entry(testString).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TestStringExists(id))
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

        [HttpPost("DataOnly")]
        public async Task<ActionResult<TestString>> PostTestString([FromForm]string data)
        {
            var testString = new TestString() {Data = data};
            _context.TestStrings.Add(testString);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTestString", new { id = testString.Id }, testString);
        }

        // POST: api/TestStrings
        [HttpPost]
        public async Task<ActionResult<TestString>> PostTestString([FromForm]TestString testString)
        {
            _context.TestStrings.Add(testString);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTestString", new { id = testString.Id }, testString);
        }

        // DELETE: api/TestStrings/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<TestString>> DeleteTestString(long id)
        {
            var testString = await _context.TestStrings.FindAsync(id);
            if (testString == null)
            {
                return NotFound();
            }

            _context.TestStrings.Remove(testString);
            await _context.SaveChangesAsync();

            return testString;
        }

        private bool TestStringExists(long id)
        {
            return _context.TestStrings.Any(e => e.Id == id);
        }
    }
}
