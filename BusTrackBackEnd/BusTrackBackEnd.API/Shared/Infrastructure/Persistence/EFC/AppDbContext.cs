using BusTrackBackEnd.API.Companies.Domain.Model.Aggregates;
using BusTrackBackEnd.API.Companies.Infrastructure.Persistence.EFC.Configuration.Extensions;
using BusTrackBackEnd.API.IAM.Domain.Model.Aggregates;
using BusTrackBackEnd.API.IAM.Infrastructure.Persistence.EFC.Configuration.Extensions;
using BusTrackBackEnd.API.Routes.Domain.Model.Entities;
using BusTrackBackEnd.API.Routes.Infrastructure.Persistence.EFC.Configuration.Extensions;
using BusTrackBackEnd.API.Shared.Infrastructure.Persistence.EFC.Configuration.Extensions;
using Microsoft.EntityFrameworkCore;
using Route = BusTrackBackEnd.API.Routes.Domain.Model.Aggregates.Route;

namespace BusTrackBackEnd.API.Shared.Infrastructure.Persistence.EFC;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options) : base(options) {}

    // IAM
    public DbSet<User> Users { get; set; }

    // Companies
    public DbSet<Company> Companies { get; set; }

    // Routes
    public DbSet<Route> Routes { get; set; }
    public DbSet<Waypoint> Waypoints { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseSnakeCase();

        base.OnModelCreating(modelBuilder);

        // IAM
        modelBuilder.ConfigureIAM();

        // Companies
        modelBuilder.ConfigureCompanies();

        // Routes
        modelBuilder.ConfigureRoutes();
    }
}