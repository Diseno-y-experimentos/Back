namespace BusTrackBackEnd.API.Companies.Domain.Model.Aggregates;

public interface ICompanyRepository
{
    Task AddAsync(Company company);
    Task<Company?> FindByIdAsync(int id);
    Task<Company?> FindByEmailAsync(string email);
    Task<IEnumerable<Company>> ListAsync();
}