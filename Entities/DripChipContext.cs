using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Entities;

public partial class DripChipContext : DbContext
{
    public DripChipContext()
    {
    }

    public DripChipContext(DbContextOptions<DripChipContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<Animal> Animals { get; set; }

    public virtual DbSet<AnimalAnimalType> AnimalAnimalTypes { get; set; }

    public virtual DbSet<AnimalType> AnimalTypes { get; set; }

    public virtual DbSet<Location> Locations { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=DripChip;Username=postgres;Password=313818");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Account_pkey");

            entity.ToTable("Account");
        });

        modelBuilder.Entity<Animal>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Animal_pkey");

            entity.ToTable("Animal");

            entity.HasOne(d => d.Chipper).WithMany(p => p.Animals)
                .HasForeignKey(d => d.ChipperId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Animal_ChipperId_fkey");

            entity.HasOne(d => d.ChippingLocation).WithMany(p => p.Animals)
                .HasForeignKey(d => d.ChippingLocationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Animal_ChippingLocationId_fkey");
        });

        modelBuilder.Entity<AnimalAnimalType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Animal_AnimalType_pkey");

            entity.ToTable("Animal_AnimalType");

            entity.HasOne(d => d.Animal).WithMany(p => p.AnimalAnimalTypes)
                .HasForeignKey(d => d.AnimalId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Animal_AnimalType_Animal_Id_fkey");

            entity.HasOne(d => d.AnimalType).WithMany(p => p.AnimalAnimalTypes)
                .HasForeignKey(d => d.AnimalTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Animal_AnimalType_AnimalType_Id_fkey");
        });

        modelBuilder.Entity<AnimalType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("AnimalType_pkey");

            entity.ToTable("AnimalType");
        });

        modelBuilder.Entity<Location>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Location_pkey");

            entity.ToTable("Location");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
