using BusTrackBackEnd.API.IAM.Domain.Model.Aggregates;
using Microsoft.EntityFrameworkCore;

namespace BusTrackBackEnd.API.IAM.Infrastructure.Persistence.EFC.Configuration.Extensions;

public static class ModelBuilderExtensions
{
    public static void ConfigureIAM(this ModelBuilder builder)
    {
        // Aquí configuramos la tabla de Usuarios
        builder.Entity<User>(entity =>
        {
            // 1. Nombre de la tabla (opcional si usas SnakeCase, pero bueno por seguridad)
            entity.ToTable("users");

            // 2. Clave primaria
            entity.HasKey(u => u.Id);
            entity.Property(u => u.Id).IsRequired().ValueGeneratedOnAdd();

            // 3. Propiedad Username (Usuario)
            entity.Property(u => u.Username)
                .IsRequired()
                .HasMaxLength(50); // Recomendado limitar longitud
            
            // IMPORTANTE: El nombre de usuario debe ser único en la BD
            entity.HasIndex(u => u.Username).IsUnique();

            // 4. Propiedad PasswordHash (La contraseña encriptada)
            entity.Property(u => u.PasswordHash)
                .IsRequired()
                .HasMaxLength(60); // BCrypt siempre genera hashes de 60 caracteres

            // Si tienes más propiedades en tu User.cs (como Roles, Email, etc.), agrégalas aquí.
        });
    }
}