using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HWWebApi.Models;
using Microsoft.AspNetCore.Mvc;

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
            return context.Computers.First(c => c.Id == id);
        }
    }
}
