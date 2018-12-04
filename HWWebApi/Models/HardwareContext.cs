using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace HWWebApi.Models
{
    public class HardwareContext : DbContext
    {
        public HardwareContext() : base() { }
        public HardwareContext(DbContextOptions<HardwareContext> options) : base(options) { }

        public DbSet<Computer> Computers { get; set; }
        public DbSet<Processor> Processors { get; set; }
    }
}