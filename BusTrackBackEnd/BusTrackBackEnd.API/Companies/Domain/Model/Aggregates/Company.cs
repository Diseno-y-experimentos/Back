namespace BusTrackBackEnd.API.Companies.Domain.Model.Aggregates;

public class Company
{
    public int Id { get; private set; }

    public string Name { get; private set; }
    public string Email { get; private set; }
    public string Ruc { get; private set; }
    public string Phone { get; private set; }
    public string Address { get; private set; }

    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    public Company(string name, string email, string ruc, string phone, string address)
    {
        Name = name;
        Email = email;
        Ruc = ruc;
        Phone = phone;
        Address = address;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    protected Company() {}
}