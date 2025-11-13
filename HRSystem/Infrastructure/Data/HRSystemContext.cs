using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using HRSystem.Domain.Entities;

namespace HRSystem.Infrastructure.Data;

public partial class HRSystemContext : IdentityDbContext
{
    public HRSystemContext()
    {
    }

    public HRSystemContext(DbContextOptions<HRSystemContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Contract> Contract { get; set; }

    public virtual DbSet<Staff> Staff { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=HRSystemContext");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Contract>(entity =>
        {
            entity.ToTable("Contract");
            entity.HasKey(e => e.Id);
        });

        modelBuilder.Entity<Staff>(entity =>
        {
            entity.ToTable("Staff");
            entity.HasKey(e => e.StaffNo);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
