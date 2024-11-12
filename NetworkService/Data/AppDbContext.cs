namespace NetworkService.Data;

using NetworkService.Models;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Network> networks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Network>().HasData(new Network
        {
            Id = 1,
            Name = "Centeral Network",
            IpAddress = "192.168.1.1",
            IsConnected = true
        });
    }
}
