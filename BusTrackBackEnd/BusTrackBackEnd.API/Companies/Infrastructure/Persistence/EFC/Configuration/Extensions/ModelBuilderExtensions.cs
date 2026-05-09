using BusTrackBackEnd.API.Companies.Domain.Model.Aggregates;
using Microsoft.EntityFrameworkCore;

namespace BusTrackBackEnd.API.Companies.Infrastructure.Persistence.EFC.Configuration.Extensions;

public static class ModelBuilderExtensions
{
    public static void ConfigureCompanies(this ModelBuilder modelBuilder)
    {
        var company = modelBuilder.Entity<Company>();

        company.ToTable("companies");

        company.HasKey(c => c.Id);

        company.Property(c => c.Name).IsRequired();
        company.Property(c => c.Email).IsRequired();
        company.Property(c => c.Ruc).IsRequired();
        company.Property(c => c.Phone).IsRequired();
        company.Property(c => c.Address).IsRequired();
    }
}