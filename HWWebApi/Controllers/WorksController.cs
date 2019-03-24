using System.Linq;
using HWWebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HWWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorksController : GenericControllerBase<Work>
    {
        public WorksController(HardwareContext context): base(context) { }

        [HttpPost]
        public ActionResult<Work> Post(string workName)
        {
            return Post(new Work {Name = workName});
        }

        public override ActionResult<Work> Get(long id)
        {
            if (context.Works.Any(w => w.Id == id))
            {
                return Ok(context.Works.Single(c=>c.Id == id));
            }
            return NotFound();
        }
    }
}
