using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HWWebApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace HWWebApi.Controllers
{
    public abstract class GenericControllerBase<T> : ControllerBase
        where T : class, IModel<T>
    {
        protected HardwareContext context;

        protected GenericControllerBase(HardwareContext context)
        {
            this.context = context;
        }

        [HttpPost]
        public ActionResult<T> Post(T model)
        {
            context.Add(model);

            if (context.SaveChanges() == 0)
            {
                return BadRequest("Error adding object");
            }

            return CreatedAtAction("Get", new { id = model.Id }, model);

        }

        public abstract ActionResult<T> Get(long id);
    }
}
