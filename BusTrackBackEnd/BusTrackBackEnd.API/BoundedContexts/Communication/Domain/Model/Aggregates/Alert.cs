using System.ComponentModel.DataAnnotations.Schema;

namespace BusTrackBackEnd.API.BoundedContexts.Communication.Domain.Model.Aggregates;

[Table("alerts")]
public class Alert
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public bool IsRead { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public Alert(string title, string message, bool isRead = false)
    {
        Title = title;
        Message = message;
        IsRead = isRead;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Update(string title, string message, bool isRead)
    {
        Title = title;
        Message = message;
        IsRead = isRead;
        UpdatedAt = DateTime.UtcNow;
    }

    protected Alert() { }
}

