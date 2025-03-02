using FutSpect.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace FutSpect.DAL;

public class FutSpectContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseNpgsql($"Server=127.0.0.1;Port=5432;Database=FutSpect;User Id=postgres;Password=postgres;");

    public DbSet<Club> Clubs { get; set; }
}
