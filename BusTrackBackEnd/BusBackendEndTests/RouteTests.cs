
using BusTrackBackEnd.API.Routes.Domain.Model.Aggregates;
using BusTrackBackEnd.API.Routes.Domain.Model.Entities;

namespace BusTrackBackEnd.Tests
{
    public class RouteTests
    {
        [Fact]
        public void Constructor_WithValidArguments_ShouldInitializePropertiesCorrectly()
        {
            string expectedName = "Ruta Troncal Norte";
            int expectedCompanyId = 5;
            int expectedEstimatedTime = 120; 
            int expectedFrequency = 15;     
            
            var route = new Route(expectedName, expectedCompanyId, expectedEstimatedTime, expectedFrequency);
            
            Assert.Equal(expectedName, route.Name);
            Assert.Equal(expectedCompanyId, route.CompanyId);
            Assert.Equal(expectedEstimatedTime, route.EstimatedTime);
            Assert.Equal(expectedFrequency, route.Frequency);
            
            Assert.NotNull(route.Waypoints);
            Assert.Empty(route.Waypoints);
            
            Assert.True(route.CreatedAt <= DateTime.UtcNow);
            Assert.True((route.UpdatedAt - route.CreatedAt).TotalSeconds < 1);
        }

        [Fact]
        public void AddWaypoint_ShouldAddWaypointToCollectionAndChangeUpdatedAt()
        {
            var route = new Route("Ruta Sur", 1, 60, 10);
            var originalUpdatedAt = route.UpdatedAt;
            
            var waypoint = new Waypoint("Paradero Inicial", -12.04, -77.02, 1);

            System.Threading.Thread.Sleep(10);
            
            route.AddWaypoint(waypoint);
            
            Assert.Single(route.Waypoints);
            Assert.Equal(waypoint, route.Waypoints.First());
            
            Assert.True(route.UpdatedAt > originalUpdatedAt);
        }

        [Fact]
        public void UpdateDetails_WithNewValues_ShouldModifyPropertiesAndChangeUpdatedAt()
        {
            var route = new Route("Ruta Vieja", 1, 60, 10);
            var originalUpdatedAt = route.UpdatedAt;
            var originalCreatedAt = route.CreatedAt;

            string newName = "Ruta Express";
            int newCompanyId = 2;
            int newEstimatedTime = 45;
            int newFrequency = 5;

            System.Threading.Thread.Sleep(10);
            
            route.UpdateDetails(newName, newCompanyId, newEstimatedTime, newFrequency);
            
            Assert.Equal(newName, route.Name);
            Assert.Equal(newCompanyId, route.CompanyId);
            Assert.Equal(newEstimatedTime, route.EstimatedTime);
            Assert.Equal(newFrequency, route.Frequency);

            Assert.True(route.UpdatedAt > originalUpdatedAt);
            Assert.Equal(originalCreatedAt, route.CreatedAt);
        }

        [Fact]
        public void ReplaceWaypoints_ShouldClearExistingAndAddNewWaypointsAndChangeUpdatedAt()
        {
            var route = new Route("Ruta Centro", 1, 30, 5);
            route.AddWaypoint(new Waypoint("Paradero Antiguo", 0, 0, 1));
            
            var originalUpdatedAt = route.UpdatedAt;
            
            var newWaypoints = new List<Waypoint>
            {
                new Waypoint("Nuevo Paradero 1", -12.1, -77.1, 1),
                new Waypoint("Nuevo Paradero 2", -12.2, -77.2, 2)
            };

            System.Threading.Thread.Sleep(10);
            
            route.ReplaceWaypoints(newWaypoints);
            
            Assert.Equal(2, route.Waypoints.Count);
            
            Assert.Contains(newWaypoints[0], route.Waypoints);
            Assert.Contains(newWaypoints[1], route.Waypoints);
            
            Assert.True(route.UpdatedAt > originalUpdatedAt);
        }
    }
}