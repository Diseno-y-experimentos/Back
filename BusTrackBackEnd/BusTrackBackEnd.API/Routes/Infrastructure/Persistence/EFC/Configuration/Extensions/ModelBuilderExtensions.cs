using BusTrackBackEnd.API.Routes.Domain.Model.Entities;
using Microsoft.EntityFrameworkCore;
using Route = BusTrackBackEnd.API.Routes.Domain.Model.Aggregates.Route;

namespace BusTrackBackEnd.API.Routes.Infrastructure.Persistence.EFC.Configuration.Extensions;

public static class ModelBuilderExtensions
{
    public static void ConfigureRoutes(this ModelBuilder modelBuilder)
    {
        // Route
        var route = modelBuilder.Entity<Route>();
        
        route.ToTable("routes");

        route.HasKey(r => r.Id);

        route.Property(r => r.Name).IsRequired();
        route.Property(r => r.CompanyId).IsRequired();

        route.HasMany(r => r.Waypoints)
            .WithOne()
            .HasForeignKey(w => w.RouteId);

        // Waypoint
        var waypoint = modelBuilder.Entity<Waypoint>();

        waypoint.ToTable("waypoints");

        waypoint.HasKey(w => w.Id);

        waypoint.Property(w => w.Name).IsRequired();
        waypoint.Property(w => w.Latitude).IsRequired();
        waypoint.Property(w => w.Longitude).IsRequired();
        waypoint.Property(w => w.Order).IsRequired();
    }
}