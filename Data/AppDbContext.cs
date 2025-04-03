using Microsoft.EntityFrameworkCore;
using CommanderGQL.Models;

namespace CommanderGQL.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options) : base(options)
    {

    }

    public DbSet<Platform> Platforms { get; set; }
    public DbSet<Command> Commands { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Platform>()
            .HasMany(p => p.Commands)
            .WithOne(p => p.Platform)
            .HasForeignKey(p => p.PlatformId);

        modelBuilder.Entity<Command>()
            .HasOne(p => p.Platform)
            .WithMany(p => p.Commands)
            .HasForeignKey(p => p.PlatformId);

        modelBuilder.Entity<Platform>().HasData(
            new Platform { Id = 1, Name = "Docker", LicenseKey = null },
            new Platform { Id = 2, Name = "Kubernetes", LicenseKey = null },
            new Platform { Id = 3, Name = ".NET 8", LicenseKey = null },
            new Platform { Id = 4, Name = "Entity Framework Core", LicenseKey = null}
        );

        modelBuilder.Entity<Command>().HasData(
            new Command { Id = 1, HowTo = "Build a dotnet project", CommandLine = "dotnet build", PlatformId = 3 },
            new Command { Id = 2, HowTo = "Run a dotnet project", CommandLine = "dotnet run", PlatformId = 3 },
            new Command { Id = 3, HowTo = "Start a docker container using docker compose", CommandLine = "docker-compose up -d", PlatformId = 1 },
            new Command { Id = 4, HowTo = "stop a docker container using docker compose", CommandLine = "docker-compose stop", PlatformId = 1 },
            new Command { Id = 5, HowTo = "Add migration", CommandLine = "dotnet ef migrations add message", PlatformId = 4 },
            new Command { Id = 6, HowTo = "Apply migration in database", CommandLine = "dotnet ef database update", PlatformId = 4 }
        );
    }
}

