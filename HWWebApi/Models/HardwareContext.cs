using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace HWWebApi.Models
{
    public class HardwareContext : DbContext
    {
        public HardwareContext(DbContextOptions<HardwareContext> options) : base(options) { }

        public DbSet<Processor> Processors { get; set; }
    }
}