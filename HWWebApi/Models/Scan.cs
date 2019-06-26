using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models;

namespace HWWebApi.Models
{
    public class Scan
    {
        public Guid Id { get; set; }
        public DateTime CreationDateTime { get; set; }
        public Computer Computer { get; set; }
        public User User { get; set; }
    }
}
