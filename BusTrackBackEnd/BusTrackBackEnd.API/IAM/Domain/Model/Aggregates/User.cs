namespace BusTrackBackEnd.API.IAM.Domain.Model.Aggregates;

public class User
{
    public int Id { get; set; }

    // Identity data
    public string Username { get; private set; }
    public string Email { get; private set; }
    public string PasswordHash { get; private set; }

    // Audit
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    public User(string username, string email, string passwordHash)
    {
        Username = username;
        Email = email;
        PasswordHash = passwordHash;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateProfile(string username, string email, string? passwordHash = null)
    {
        Username = username;
        Email = email;

        if (!string.IsNullOrWhiteSpace(passwordHash))
        {
            PasswordHash = passwordHash;
        }

        UpdatedAt = DateTime.UtcNow;
    }

    protected User() {}
}