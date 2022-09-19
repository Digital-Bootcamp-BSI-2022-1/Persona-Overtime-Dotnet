using Microsoft.EntityFrameworkCore;
using dotnet_2.Infrastructure.Data.Models;
using System.Reflection;


namespace dotnet_2.Infrastructure.Data;
class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<AuthTokenn> AuthTokenns { get; set; }
    public DbSet<Organization> Organizations { get; set; }
    public DbSet<Overtime> Overtime { get; set; }
    public DbSet<WorkSchedule> WorkSchedules { get; set; }

    public string DbPath { get; }

    public AppDbContext(DbContextOptions options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        modelBuilder.Entity<User>().HasOne(p => p.organization).WithMany(b => b.member);
    }
        

}