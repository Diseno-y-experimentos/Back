using BusTrackBackEnd.API.BoundedContexts.Communication.Domain.Model.Aggregates;

namespace BusTrackBackEnd.Tests
{
    public class NotificationTests
    {
        [Fact]
        public void Constructor_WithValidArguments_ShouldInitializePropertiesCorrectly()
        {
            var expectedTitle = "Nuevo mensaje del administrador";
            var expectedMessage = "Tienes una nueva actualización disponible.";
            var expectedIsRead = false;
            
            var notification = new Notification(expectedTitle, expectedMessage, expectedIsRead);
            
            Assert.Equal(expectedTitle, notification.Title);
            Assert.Equal(expectedMessage, notification.Message);
            Assert.Equal(expectedIsRead, notification.IsRead);
            
            Assert.True(notification.CreatedAt <= DateTime.UtcNow);
            Assert.True((notification.UpdatedAt - notification.CreatedAt).TotalSeconds < 1);
        }

        [Fact]
        public void Update_WithNewValues_ShouldModifyPropertiesAndChangeUpdatedAt()
        {
            var notification = new Notification("Título Original", "Mensaje Original", false);
            var originalUpdatedAt = notification.UpdatedAt;
            
            var newTitle = "Recordatorio de viaje";
            var newMessage = "Tu bus sale en 1 hora.";
            var newIsRead = true;
            
            System.Threading.Thread.Sleep(10);
            
            notification.Update(newTitle, newMessage, newIsRead);
            
            Assert.Equal(newTitle, notification.Title);
            Assert.Equal(newMessage, notification.Message);
            Assert.Equal(newIsRead, notification.IsRead);
            
            Assert.True(notification.UpdatedAt > originalUpdatedAt);
            
            Assert.True(notification.UpdatedAt > notification.CreatedAt);
        }
    }
}