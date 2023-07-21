using System;
using System.Collections.Generic;
using Astara_API.DataAccess.mytasks.Model;
using Microsoft.EntityFrameworkCore;

namespace Astara_API.DataAccess.mytasks.Context;

public partial class myTasksContext : DbContext
{
    public myTasksContext()
    {
    }

    public myTasksContext(DbContextOptions<myTasksContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //    => optionsBuilder.UseSqlServer("Name=ConnectionStrings:myTasksDB");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Role>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Active)
                .IsRequired()
                .HasDefaultValueSql("((1))");
            entity.Property(e => e.Rol)
                .HasMaxLength(25)
                .IsUnicode(false);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Apellidos).HasMaxLength(50);
            entity.Property(e => e.Nombre).HasMaxLength(30);
            entity.Property(e => e.Password).HasMaxLength(25);
            entity.Property(e => e.Usuario).HasMaxLength(25);

            entity.HasOne(d => d.RolNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.Rol)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Users_Roles");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
