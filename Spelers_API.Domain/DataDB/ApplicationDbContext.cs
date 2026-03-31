using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Spelers_API.Domain.EntitiesDB;

namespace Spelers_API.Domain.DataDB;

public class ApplicationDbContext : IdentityDbContext<IdentityUser>
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Positie> Posities { get; set; }
    public virtual DbSet<Speler> Spelers { get; set; }
    public virtual DbSet<Team> Teams { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning Move your connection string to configuration
        => optionsBuilder.UseSqlServer("Server=.\\SQL22_VIVES; Database=spelersSQL; Trusted_Connection=True; TrustServerCertificate=True; MultipleActiveResultSets=true;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder); // ✅ REQUIRED

        modelBuilder.Entity<Positie>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.ToTable("Positie");

            entity.Property(e => e.Naam)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Speler>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.ToTable("Speler");

            entity.Property(e => e.Naam)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Positie)
                .WithMany(p => p.Spelers)
                .HasForeignKey(d => d.PositieId);

            entity.HasOne(d => d.Team)
                .WithMany(p => p.Spelers)
                .HasForeignKey(d => d.TeamId);
        });

        modelBuilder.Entity<Team>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.ToTable("Team");

            entity.Property(e => e.Naam)
                .HasMaxLength(50)
                .IsUnicode(false);
        });
    }
}