using System;
using System.Reflection;
using ClassifiedsApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL;

namespace ClassifiedsApi.DataAccess;

public class DataBaseContext : DbContext
{
    private readonly string _connectionString;

    public DataBaseContext(string connectionString) : base()
    {
        _connectionString = connectionString;
    }

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Advert> Adverts { get; set; } = null!;
    public DbSet<Comment> Comments { get; set; } = null!;
    public DbSet<Category> Categories { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(_connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DataBaseContext).Assembly);
    }
}
