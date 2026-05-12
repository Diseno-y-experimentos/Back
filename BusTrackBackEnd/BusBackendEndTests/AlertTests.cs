using BusTrackBackEnd.API.BoundedContexts.Communication.Domain.Model.Aggregates;

namespace BusTrackBackEnd.Tests
{
    public class AlertTests
    {
        [Fact]
        public void Constructor_WithValidArguments_ShouldInitializePropertiesCorrectly()
        {
            var expectedTitle = "Nueva alerta de sistema";
            var expectedMessage = "El servidor se reiniciará en 5 minutos.";
            var expectedIsRead = false;
            
            var alert = new Alert(expectedTitle, expectedMessage, expectedIsRead);
            
            Assert.Equal(expectedTitle, alert.Title);
            Assert.Equal(expectedMessage, alert.Message);
            Assert.Equal(expectedIsRead, alert.IsRead);
            
            Assert.True(alert.CreatedAt <= DateTime.UtcNow);
            
            Assert.True((alert.UpdatedAt - alert.CreatedAt).TotalSeconds < 1);
        }

        [Fact]
        public void Update_WithNewValues_ShouldModifyPropertiesAndChangeUpdatedAt()
        {
            var alert = new Alert("Título Original", "Mensaje Original", false);
            var originalUpdatedAt = alert.UpdatedAt;
            
            var newTitle = "Título Actualizado";
            var newMessage = "Mensaje Actualizado";
            var newIsRead = true;
            
            System.Threading.Thread.Sleep(10);
            
            alert.Update(newTitle, newMessage, newIsRead);
            
            Assert.Equal(newTitle, alert.Title);
            Assert.Equal(newMessage, alert.Message);
            Assert.Equal(newIsRead, alert.IsRead);
            
            Assert.True(alert.UpdatedAt > originalUpdatedAt);
            
            Assert.True(alert.UpdatedAt > alert.CreatedAt);
        }
    }
}