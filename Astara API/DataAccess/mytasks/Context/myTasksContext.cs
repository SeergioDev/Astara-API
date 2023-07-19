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

    public virtual DbSet<User> Users { get; set; }

    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //    => optionsBuilder.UseSqlServer("Name=ConnectionStrings:myTasksDB");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .IsFixedLength();
            entity.Property(e => e.Usuario)
                .HasMaxLength(50)
                .IsFixedLength()
                .HasColumnName("Usuario");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
