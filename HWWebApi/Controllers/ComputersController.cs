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
        public void Post([FromBody] Computer computer)
        {
        }
    }
}
