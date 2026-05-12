
using BusTrackBackEnd.API.BoundedContexts.Transport.Domain.Model;

namespace BusTrackBackEnd.Tests
{
    public class BusTests
    {

        [Fact]
        public void Constructor_WithValidArguments_ShouldInitializePropertiesCorrectly()
        {
            string expectedPlate = "ABC-123";
            int expectedCapacity = 40;
            string expectedLocation = "Terminal Norte";
            
            var bus = new Bus(expectedPlate, expectedCapacity, expectedLocation);
            
            Assert.Equal(expectedPlate, bus.LicensePlate);
            Assert.Equal(expectedCapacity, bus.Capacity);
            Assert.Equal(expectedLocation, bus.CurrentLocation);
            
            Assert.Equal(BusStatus.INACTIVE, bus.Status);
            Assert.Null(bus.CurrentRouteId);
        }

        [Fact]
        public void Constructor_WithNullLicensePlate_ShouldThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new Bus(null, 40, "Terminal Norte"));
        }

        [Fact]
        public void Constructor_WithZeroOrNegativeCapacity_ShouldThrowArgumentException()
        {
            Assert.Throws<ArgumentException>(() => new Bus("ABC-123", 0, "Terminal Norte"));
            Assert.Throws<ArgumentException>(() => new Bus("ABC-123", -5, "Terminal Norte"));
        }

        [Fact]
        public void Constructor_WithNullLocation_ShouldThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new Bus("ABC-123", 40, null));
        }
        

        [Fact]
        public void UpdateLocation_WithValidLocation_ShouldUpdateProperty()
        {
            var bus = new Bus("ABC-123", 40, "Terminal Norte");
            string newLocation = "Avenida Javier Prado";
            
            bus.UpdateLocation(newLocation);
            
            Assert.Equal(newLocation, bus.CurrentLocation);
        }

        [Fact]
        public void UpdateLocation_WithNullLocation_ShouldThrowArgumentNullException()
        {
            var bus = new Bus("ABC-123", 40, "Terminal Norte");
            
            Assert.Throws<ArgumentNullException>(() => bus.UpdateLocation(null));
        }

        [Fact]
        public void ChangeStatus_WithNewStatus_ShouldUpdateProperty()
        {
            var bus = new Bus("ABC-123", 40, "Terminal Norte");
            
            bus.ChangeStatus(BusStatus.ACTIVE); 
            
            Assert.Equal(BusStatus.ACTIVE, bus.Status);
        }

        [Fact]
        public void AssignRoute_WithValidRouteId_ShouldAssignRouteAndSetActiveStatus()
        {
            var bus = new Bus("ABC-123", 40, "Terminal Norte");
            int routeId = 15;
            
            bus.AssignRoute(routeId);
            
            Assert.Equal(routeId, bus.CurrentRouteId);
            Assert.Equal(BusStatus.ACTIVE, bus.Status);
        }
    }
}