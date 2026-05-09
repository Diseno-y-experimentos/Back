using Microsoft.EntityFrameworkCore;

namespace BusTrackBackEnd.API.Shared.Infrastructure.Persistence.EFC.Configuration.Extensions;

public static class ModelBuilderExtensions
{
    public static void UseSnakeCase(this ModelBuilder modelBuilder)
    {
        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            entity.SetTableName(ToSnakeCase(entity.GetTableName()));

            foreach (var property in entity.GetProperties())
                property.SetColumnName(ToSnakeCase(property.Name));

            foreach (var key in entity.GetKeys())
                key.SetName(ToSnakeCase(key.GetName()));

            foreach (var fk in entity.GetForeignKeys())
                fk.SetConstraintName(ToSnakeCase(fk.GetConstraintName()));

            foreach (var index in entity.GetIndexes())
                index.SetDatabaseName(ToSnakeCase(index.GetDatabaseName()));
        }
    }

    private static string? ToSnakeCase(string? name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return name;

        var builder = new System.Text.StringBuilder();
        for (int i = 0; i < name.Length; i++)
        {
            var c = name[i];
            if (char.IsUpper(c))
            {
                if (i > 0) builder.Append('_');
                builder.Append(char.ToLowerInvariant(c));
            }
            else builder.Append(c);
        }
        return builder.ToString();
    }
}