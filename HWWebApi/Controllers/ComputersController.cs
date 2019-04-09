using System.Linq;
using HWWebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HWWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComputersController : GenericControllerBase<Computer>
    {
        public ComputersController(HardwareContext context) : base(context) { }

        // POST api/values
        [HttpPost("Form")]
        public ActionResult<Computer> PostForm([FromForm] Computer computer)
        {
            return Post(computer);
        }

        // POST api/values
        [HttpPost("Body")]
        [ProducesResponseType(201, Type = typeof(Computer))]
        public ActionResult<Computer> PostBody([FromBody] Computer computer)
        {
            return Post(computer);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public override ActionResult<Computer> Get(long id)
        {
            if (context.Computers.Any(c => c.Id == id))
            {
                return Ok(context.Computers
                .Include(c => c.Disks)
                .Include(c => c.Gpus)
                .Include(c => c.Memories)
                .Include(c => c.MotherBoard)
                .Include(c => c.Processor)
                .Single(c => c.Id == id));
            }
            return NotFound();
        }
    }
}
