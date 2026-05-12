using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusTrackBackEnd.API.Migrations
{
    /// <inheritdoc />
    public partial class AddNotificationsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // This migration originally contained many CreateTable calls that may duplicate
            // existing schema in the database. To avoid conflicts when applying to an
            // already-initialized database, we only ensure the notifications table exists.

            migrationBuilder.Sql(
                "CREATE TABLE IF NOT EXISTS `notifications` (\n" +
                "  `id` int NOT NULL AUTO_INCREMENT,\n" +
                "  `title` longtext CHARACTER SET utf8mb4 NOT NULL,\n" +
                "  `message` longtext CHARACTER SET utf8mb4 NOT NULL,\n" +
                "  `is_read` tinyint(1) NOT NULL,\n" +
                "  `created_at` datetime(6) NOT NULL,\n" +
                "  `updated_at` datetime(6) NOT NULL,\n" +
                "  PRIMARY KEY (`id`)\n" +
                ") CHARACTER SET=utf8mb4;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP TABLE IF EXISTS `notifications`;");
        }
    }
}
