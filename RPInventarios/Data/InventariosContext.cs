using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RPInventarios.Models;

namespace RPInventarios.Data;

public class InventariosContext : DbContext
{
    public InventariosContext (DbContextOptions<InventariosContext> options)
        : base(options)
    {
    }

    public DbSet<Marca> Marca { get; set; }
    public DbSet<Departamento> Departamento { get; set; }
    public DbSet<Perfil> Perfil { get; set; }
    public DbSet<Producto> Producto { get; set; }
    public DbSet<Usuario> Usuario { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Marca>().ToTable("Marca");
        modelBuilder.Entity<Departamento>().ToTable("Departamento");
        modelBuilder.Entity<Perfil>().ToTable("Perfil");
        modelBuilder.Entity<Producto>().ToTable("Producto");
        modelBuilder.Entity<Usuario>().ToTable("Usuario");

        base.OnModelCreating(modelBuilder);
    }
}
