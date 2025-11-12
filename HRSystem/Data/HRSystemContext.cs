using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using HRSystem.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace HRSystem.Data;

public partial class HRSystemContext : IdentityDbContext
{
    public HRSystemContext()
    {
    }

    public HRSystemContext(DbContextOptions<HRSystemContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ContractModel> Contract { get; set; }

    public virtual DbSet<StaffModel> Staff { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=HRSystemContext");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ContractModel>(entity =>
        {
            entity.ToTable("Contract");
            entity.HasKey(e => e.Id);
        });

        modelBuilder.Entity<StaffModel>(entity =>
        {
            entity.ToTable("Staff");
            entity.HasKey(e => e.StaffNo);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
