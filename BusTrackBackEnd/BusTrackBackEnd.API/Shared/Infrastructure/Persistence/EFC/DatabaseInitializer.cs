using Microsoft.EntityFrameworkCore;

namespace BusTrackBackEnd.API.Shared.Infrastructure.Persistence.EFC;

public static class DatabaseInitializer
{
    public static async Task EnsureAdditionalTablesAsync(AppDbContext context)
    {
        await context.Database.ExecuteSqlRawAsync(@"
CREATE TABLE IF NOT EXISTS `saved_routes` (
    `id` int NOT NULL AUTO_INCREMENT,
    `user_id` int NOT NULL,
    `route_id` int NOT NULL,
    `created_at` datetime(6) NOT NULL,
    `updated_at` datetime(6) NOT NULL,
    PRIMARY KEY (`id`),
    KEY `ix_saved_routes_user_id` (`user_id`),
    KEY `ix_saved_routes_route_id` (`route_id`),
    CONSTRAINT `fk_saved_routes_users_user_id` FOREIGN KEY (`user_id`) REFERENCES `users` (`id`) ON DELETE CASCADE,
    CONSTRAINT `fk_saved_routes_routes_route_id` FOREIGN KEY (`route_id`) REFERENCES `routes` (`id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;");

        await context.Database.ExecuteSqlRawAsync(@"
CREATE TABLE IF NOT EXISTS `trips` (
    `id` int NOT NULL AUTO_INCREMENT,
    `user_id` int NOT NULL,
    `route_id` int NULL,
    `origin` longtext NULL,
    `destination` longtext NULL,
    `started_at` datetime(6) NULL,
    `ended_at` datetime(6) NULL,
    `notes` longtext NULL,
    `created_at` datetime(6) NOT NULL,
    `updated_at` datetime(6) NOT NULL,
    PRIMARY KEY (`id`),
    KEY `ix_trips_user_id` (`user_id`),
    KEY `ix_trips_route_id` (`route_id`),
    CONSTRAINT `fk_trips_users_user_id` FOREIGN KEY (`user_id`) REFERENCES `users` (`id`) ON DELETE CASCADE,
    CONSTRAINT `fk_trips_routes_route_id` FOREIGN KEY (`route_id`) REFERENCES `routes` (`id`) ON DELETE SET NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;");

        await context.Database.ExecuteSqlRawAsync(@"
CREATE TABLE IF NOT EXISTS `alerts` (
    `id` int NOT NULL AUTO_INCREMENT,
    `title` longtext NOT NULL,
    `message` longtext NOT NULL,
    `is_read` tinyint(1) NOT NULL,
    `created_at` datetime(6) NOT NULL,
    `updated_at` datetime(6) NOT NULL,
    PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;");

        await context.Database.ExecuteSqlRawAsync(@"
CREATE TABLE IF NOT EXISTS `notifications` (
    `id` int NOT NULL AUTO_INCREMENT,
    `title` longtext NOT NULL,
    `message` longtext NOT NULL,
    `is_read` tinyint(1) NOT NULL,
    `created_at` datetime(6) NOT NULL,
    `updated_at` datetime(6) NOT NULL,
    PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;");
    }
}

