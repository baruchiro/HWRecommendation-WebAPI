using Models;
using System;

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
