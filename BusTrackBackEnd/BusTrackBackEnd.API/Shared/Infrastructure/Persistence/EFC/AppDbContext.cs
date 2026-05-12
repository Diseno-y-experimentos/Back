using BusTrackBackEnd.API.Companies.Domain.Model.Aggregates;
using BusTrackBackEnd.API.Companies.Infrastructure.Persistence.EFC.Configuration.Extensions;
using BusTrackBackEnd.API.BoundedContexts.Communication.Domain.Model.Aggregates;
using BusTrackBackEnd.API.IAM.Domain.Model.Aggregates;
using BusTrackBackEnd.API.IAM.Infrastructure.Persistence.EFC.Configuration.Extensions;
using BusTrackBackEnd.API.BoundedContexts.Users.Domain.Model.Aggregates;
using BusTrackBackEnd.API.Routes.Domain.Model.Entities;
using BusTrackBackEnd.API.Routes.Infrastructure.Persistence.EFC.Configuration.Extensions;
using BusTrackBackEnd.API.Shared.Infrastructure.Persistence.EFC.Configuration.Extensions;
using BusTrackBackEnd.API.BoundedContexts.Transport.Domain.Model;
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

    // Transport
    public DbSet<Bus> Buses { get; set; }

    // User interactions
    public DbSet<SavedRoute> SavedRoutes { get; set; }
    public DbSet<Trip> Trips { get; set; }

    // Communication
    public DbSet<Alert> Alerts { get; set; }
    public DbSet<Notification> Notifications { get; set; }

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