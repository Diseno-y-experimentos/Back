
using BusTrackBackEnd.API.BoundedContexts.Users.Domain.Model.Aggregates;

namespace BusTrackBackEnd.Tests
{
    public class TripTests
    {
        [Fact]
        public void Constructor_WithAllArguments_ShouldInitializePropertiesCorrectly()
        {
            int expectedUserId = 1;
            int? expectedRouteId = 10;
            string expectedOrigin = "Terminal Sur";
            string expectedDestination = "Miraflores";
            DateTime? expectedStartedAt = DateTime.UtcNow.AddMinutes(-30);
            DateTime? expectedEndedAt = null; 
            string expectedNotes = "Viaje normal sin retrasos";
            
            var trip = new Trip(
                expectedUserId, 
                expectedRouteId, 
                expectedOrigin, 
                expectedDestination, 
                expectedStartedAt, 
                expectedEndedAt, 
                expectedNotes
            );
            
            Assert.Equal(expectedUserId, trip.UserId);
            Assert.Equal(expectedRouteId, trip.RouteId);
            Assert.Equal(expectedOrigin, trip.Origin);
            Assert.Equal(expectedDestination, trip.Destination);
            Assert.Equal(expectedStartedAt, trip.StartedAt);
            Assert.Equal(expectedEndedAt, trip.EndedAt);
            Assert.Equal(expectedNotes, trip.Notes);
            
            Assert.True(trip.CreatedAt <= DateTime.UtcNow);
            Assert.True((trip.UpdatedAt - trip.CreatedAt).TotalSeconds < 1);
        }

        [Fact]
        public void Constructor_WithNullOptionalArguments_ShouldInitializeCorrectly()
        {
            int expectedUserId = 2; 
            
            var trip = new Trip(expectedUserId, null, null, null, null, null, null);
            
            Assert.Equal(expectedUserId, trip.UserId);
            Assert.Null(trip.RouteId);
            Assert.Null(trip.Origin);
            Assert.Null(trip.Destination);
            Assert.Null(trip.StartedAt);
            Assert.Null(trip.EndedAt);
            Assert.Null(trip.Notes);
            
            Assert.True(trip.CreatedAt <= DateTime.UtcNow);
        }

        [Fact]
        public void Update_WithNewValues_ShouldModifyPropertiesAndChangeUpdatedAt()
        {
            var trip = new Trip(1, null, "Origen Antiguo", null, null, null, null);
            var originalUpdatedAt = trip.UpdatedAt;
            var originalCreatedAt = trip.CreatedAt;
            
            int? newRouteId = 5;
            string newOrigin = "Nuevo Origen";
            string newDestination = "Nuevo Destino";
            DateTime? newStartedAt = DateTime.UtcNow.AddHours(-1);
            DateTime? newEndedAt = DateTime.UtcNow;
            string newNotes = "Viaje finalizado con éxito";
            
            System.Threading.Thread.Sleep(10);
            
            trip.Update(newRouteId, newOrigin, newDestination, newStartedAt, newEndedAt, newNotes);
            
            Assert.Equal(newRouteId, trip.RouteId);
            Assert.Equal(newOrigin, trip.Origin);
            Assert.Equal(newDestination, trip.Destination);
            Assert.Equal(newStartedAt, trip.StartedAt);
            Assert.Equal(newEndedAt, trip.EndedAt);
            Assert.Equal(newNotes, trip.Notes);
            
            Assert.True(trip.UpdatedAt > originalUpdatedAt);
            Assert.Equal(originalCreatedAt, trip.CreatedAt); 
        }
    }
}