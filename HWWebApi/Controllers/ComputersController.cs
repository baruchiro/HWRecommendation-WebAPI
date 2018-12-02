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
        // POST api/values
        [HttpPost]
        public long Post([FromBody] Computer computer)
        {
            using (var context = new HardwareContext())
            {
                context.Computers.Add(computer);
                context.SaveChanges();
            }
            return computer.Id;
        }

        [HttpGet]
        public Computer Get(long id)
        {
            using (var context = new HardwareContext())
            {
                return context.Computers.First(c => c.Id == id);
            }
        }
    }
}
