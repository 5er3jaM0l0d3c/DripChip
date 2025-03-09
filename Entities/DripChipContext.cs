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

    public virtual DbSet<Account> Account { get; set; }

    public virtual DbSet<Animal> Animal { get; set; }

    public virtual DbSet<AnimalAnimalType> Animal_AnimalType { get; set; }

    public virtual DbSet<AnimalLocation> Animal_Location { get; set; }

    public virtual DbSet<AnimalType> AnimalType { get; set; }

    public virtual DbSet<Location> Location { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=db;Port=5432;Database=DripChip;Username=postgres;Password=313818");

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

            entity.Property(e => e.ChippingDateTime)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp(0) without time zone");
            entity.Property(e => e.DeathDateTime).HasColumnType("timestamp(0) without time zone");

            entity.Ignore(e => e.VisitedLocations);
            entity.Ignore(e => e.AnimalTypes);

        });

        modelBuilder.Entity<AnimalAnimalType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Animal_AnimalType_pkey");

            entity.ToTable("Animal_AnimalType");
        });

        modelBuilder.Entity<AnimalLocation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Animal_Location_pkey");

            entity.ToTable("Animal_Location");

            entity.Property(e => e.DateTimeOfVisitLocationPoint)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp(0) without time zone");
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
