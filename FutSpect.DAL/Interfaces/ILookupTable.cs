using Microsoft.EntityFrameworkCore;

namespace FutSpect.DAL.Interfaces;

public interface ILookupTable
{
    int Id { get; }
    string Name { get; }
    void SeedData(DbContext dbContext);
    Task SeedDataAsync(DbContext dbContext, CancellationToken cancellationToken);
}