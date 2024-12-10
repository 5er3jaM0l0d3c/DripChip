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

    public virtual DbSet<AnimalAnimalType> AnimalAnimalType { get; set; }

    public virtual DbSet<AnimalType> AnimalType { get; set; }

    public virtual DbSet<Location> Location { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=DripChip;Username=postgres;Password=313818");

}
