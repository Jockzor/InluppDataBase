
using _01.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace _01.Context;

internal class ApplicationDbContext : DbContext
{

    public DbSet<PersonEntity> People { get; set; }
    public DbSet<AddressEntity> Addresses { get; set; }
    public DbSet<RoleEntity> Roles { get; set; }
    public DbSet<ProductEntity> Products { get; set; }
    public DbSet<CategoryEntity> Categories { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Nackademin\DataBasTeknik\ConsoleAppToDataBase\01\Context\01DataBase.mdf;Integrated Security=True;Connect Timeout=30");
      // optionsBuilder.UseSqlServer(@"");
    }
}
