using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HWWebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HWWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComputersController : ControllerBase
    {
        private HardwareContext context = new HardwareContext();

        public ComputersController(HardwareContext context)
        {
            this.context = context;
        }

        // POST api/values
        [HttpPost]
        public ActionResult Post([FromBody] Computer computer)
        {
            context.Computers.Add(computer);
            context.SaveChanges();

            return CreatedAtAction("Get", new { id = computer.Id });
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<Computer> Get(long id)
        {
            return context.Computers
            .Include(c => c.Disks)
            .Include(c => c.GPUs)
            .Include(c => c.Memories)
            .Include(c => c.MotherBoard)
            .Include(c => c.Processor)
            .First(c => c.Id == id);
        }
    }
}
