
using BusTrackBackEnd.API.Companies.Domain.Model.Aggregates;

namespace BusTrackBackEnd.Tests
{
    public class CompanyTests
    {
        [Fact]
        public void Constructor_WithValidArguments_ShouldInitializePropertiesCorrectly()
        {
            string expectedName = "Transportes Expreso S.A.C.";
            string expectedEmail = "contacto@expreso.com.pe";
            string expectedRuc = "20123456789";
            string expectedPhone = "987654321";
            string expectedAddress = "Av. Javier Prado Este 1234, San Isidro";
            
            var company = new Company(expectedName, expectedEmail, expectedRuc, expectedPhone, expectedAddress);
            
            Assert.Equal(expectedName, company.Name);
            Assert.Equal(expectedEmail, company.Email);
            Assert.Equal(expectedRuc, company.Ruc);
            Assert.Equal(expectedPhone, company.Phone);
            Assert.Equal(expectedAddress, company.Address);
            
            Assert.True(company.CreatedAt <= DateTime.UtcNow);
            
            Assert.True((company.UpdatedAt - company.CreatedAt).TotalSeconds < 1);
        }

        [Fact]
        public void UpdateDetails_WithNewValues_ShouldModifyPropertiesAndChangeUpdatedAt()
        {
            var company = new Company(
                "Empresa Antigua", 
                "viejo@correo.com", 
                "10987654321", 
                "111222333", 
                "Calle Falsa 123"
            );
            
            var originalUpdatedAt = company.UpdatedAt;
            var originalCreatedAt = company.CreatedAt;
            
            string newName = "Nueva Razón Social EIRL";
            string newEmail = "nuevo@correo.com";
            string newRuc = "20999888777";
            string newPhone = "999888777";
            string newAddress = "Av. Brasil 456, Magdalena";
            
            System.Threading.Thread.Sleep(10);
            
            company.UpdateDetails(newName, newEmail, newRuc, newPhone, newAddress);
            
            Assert.Equal(newName, company.Name);
            Assert.Equal(newEmail, company.Email);
            Assert.Equal(newRuc, company.Ruc);
            Assert.Equal(newPhone, company.Phone);
            Assert.Equal(newAddress, company.Address);
            
            Assert.True(company.UpdatedAt > originalUpdatedAt);
            
            Assert.Equal(originalCreatedAt, company.CreatedAt);
        }
    }
}