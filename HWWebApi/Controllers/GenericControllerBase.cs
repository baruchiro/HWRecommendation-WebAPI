using HWWebApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace HWWebApi.Controllers
{
    public abstract class GenericControllerBase<T> : ControllerBase
        where T : class, IModel<T>
    {
        protected HardwareContext Context;

        protected GenericControllerBase(HardwareContext context)
        {
            this.Context = context;
        }

        [HttpPost]
        public ActionResult<T> Post(T model)
        {
            Context.Add(model);

            if (Context.SaveChanges() == 0)
            {
                return BadRequest("Error adding object");
            }

            return CreatedAtAction("Get", new { id = model.Id }, model);

        }

        public abstract ActionResult<T> Get(long id);
    }
}
