using Microsoft.EntityFrameworkCore;

namespace HWWebApi.Models
{
    public class HardwareContext : DbContext
    {
        public HardwareContext() : base() { }
        public HardwareContext(DbContextOptions<HardwareContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<UserChannel>()
                .HasIndex(uc => new { uc.ChannelId, uc.UserId })
                .IsUnique();
        }

        public DbSet<Computer> Computers { get; set; }
        public DbSet<Processor> Processors { get; set; }
        public DbSet<Disk> Disks { get; set; }
        public DbSet<Memory> Memories { get; set; }
        public DbSet<MotherBoard> MotherBoards { get; set; }
        public DbSet<Gpu> GPUs { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserChannel> UserChannels { get; set; }
        public DbSet<TestString> TestStrings { get; set; }
    }
}