using BusTrackBackEnd.API.BoundedContexts.Users.Domain.Model.Aggregates;


namespace BusTrackBackEnd.Tests
{
    public class SavedRouteTests
    {
        [Fact]
        public void Constructor_WithValidArguments_ShouldInitializePropertiesCorrectly()
        {
            int expectedUserId = 42;
            int expectedRouteId = 15;
            
            var savedRoute = new SavedRoute(expectedUserId, expectedRouteId);
            
            Assert.Equal(expectedUserId, savedRoute.UserId);
            Assert.Equal(expectedRouteId, savedRoute.RouteId);
            
            Assert.True(savedRoute.CreatedAt <= System.DateTime.UtcNow);
            
            Assert.True((savedRoute.UpdatedAt - savedRoute.CreatedAt).TotalSeconds < 1);
        }

        [Fact]
        public void Touch_ShouldChangeUpdatedAt_AndLeaveCreatedAtIntact()
        {
            var savedRoute = new SavedRoute(1, 1);
            var originalUpdatedAt = savedRoute.UpdatedAt;
            var originalCreatedAt = savedRoute.CreatedAt;
            
            System.Threading.Thread.Sleep(10);
            
            savedRoute.Touch();
            
            Assert.True(savedRoute.UpdatedAt > originalUpdatedAt);
            
            Assert.Equal(originalCreatedAt, savedRoute.CreatedAt);
        }
    }
}