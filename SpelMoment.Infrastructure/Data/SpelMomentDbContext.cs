using Microsoft.EntityFrameworkCore;
using SpelMoment.Domain.Model;

namespace SpelMoment.Infrastructure.Data;

public class SpelMomentDbContext : DbContext
{
    public const string SqlSchema = "SpelMoment";

    public SpelMomentDbContext(DbContextOptions<SpelMomentDbContext> options) : base(options)
    {
    }

    public DbSet<Moment> Moments => Set<Moment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder) => modelBuilder.HasDefaultSchema(SqlSchema);
}