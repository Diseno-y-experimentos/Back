using System.ComponentModel.DataAnnotations.Schema;

namespace BusTrackBackEnd.API.BoundedContexts.Communication.Domain.Model.Aggregates;

[Table("notifications")]
public class Notification
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public bool IsRead { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public Notification(string title, string message, bool isRead = false)
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

    protected Notification() { }
}

