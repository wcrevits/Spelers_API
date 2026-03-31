using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Spelers_API.Domain.EntitiesDB;

namespace Spelers_API.Domain.DataDB;

public partial class SpelerDbContext : DbContext
{
    public SpelerDbContext()
    {
    }

    public SpelerDbContext(DbContextOptions<SpelerDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Positie> Posities { get; set; }

    public virtual DbSet<Speler> Spelers { get; set; }

    public virtual DbSet<Team> Teams { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=.\\SQL22_VIVES; Database=spelersSQL; Trusted_Connection=True; TrustServerCertificate=True; MultipleActiveResultSets=true;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Positie>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Positie__3214EC0773ABB1A1");

            entity.ToTable("Positie");

            entity.Property(e => e.Naam)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Speler>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Speler__3214EC07C6D3C59D");

            entity.ToTable("Speler");

            entity.Property(e => e.Naam)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Positie).WithMany(p => p.Spelers)
                .HasForeignKey(d => d.PositieId)
                .HasConstraintName("FK_Speler_Positie");

            entity.HasOne(d => d.Team).WithMany(p => p.Spelers)
                .HasForeignKey(d => d.TeamId)
                .HasConstraintName("FK_Speler_Team");
        });

        modelBuilder.Entity<Team>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Team__3214EC07DB2B1A35");

            entity.ToTable("Team");

            entity.Property(e => e.Naam)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
