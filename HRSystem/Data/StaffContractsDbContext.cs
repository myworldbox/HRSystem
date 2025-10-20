using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using HRSystem.Models;

namespace HRSystem.Data;

public partial class StaffContractsDbContext : DbContext
{
    public StaffContractsDbContext()
    {
    }

    public StaffContractsDbContext(DbContextOptions<StaffContractsDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ContractModel> Contract { get; set; }

    public virtual DbSet<StaffModel> Staff { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=StaffContractsDbContext");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ContractModel>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Contract__3214EC07877C453C");

            entity.Property(e => e.ContractNumber).HasComputedColumnSql("(CONVERT([nvarchar](7),(format(datepart(year,getdate())%(100),'00')+'-')+format([Id],'0000')))", false);

            entity.HasOne(d => d.StaffNoNavigation).WithMany(p => p.Contracts)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Contract__StaffN__1BC821DD");
        });

        modelBuilder.Entity<StaffModel>(entity =>
        {
            entity.HasKey(e => e.StaffNo).HasName("PK__Staff__96D4E2235808A97B");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
