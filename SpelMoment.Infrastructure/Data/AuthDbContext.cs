using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SpelMoment.Domain.Model;

namespace SpelMoment.Infrastructure.Data;

public class AuthDbContext : IdentityDbContext
{
    
    public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
    {
    }
    
    public const string SqlSchema = "SpelMoment_Auth";

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.HasDefaultSchema(SqlSchema);
        base.OnModelCreating(builder);
    }
}