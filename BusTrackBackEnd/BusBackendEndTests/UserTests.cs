
using BusTrackBackEnd.API.IAM.Domain.Model.Aggregates;

namespace BusTrackBackEnd.Tests
{
    public class UserTests
    {
        [Fact]
        public void Constructor_WithValidArguments_ShouldInitializePropertiesCorrectly()
        {
            string expectedUsername = "admin_test";
            string expectedEmail = "admin@bustrack.com";
            string expectedHash = "hashed_password_123";
            
            var user = new User(expectedUsername, expectedEmail, expectedHash);
            
            Assert.Equal(expectedUsername, user.Username);
            Assert.Equal(expectedEmail, user.Email);
            Assert.Equal(expectedHash, user.PasswordHash);
            
            Assert.True(user.CreatedAt <= DateTime.UtcNow);
            Assert.True((user.UpdatedAt - user.CreatedAt).TotalSeconds < 1);
        }

        [Fact]
        public void UpdateProfile_WithNewPasswordHash_ShouldUpdateAllFieldsAndUpdatedAt()
        {
            var user = new User("old_user", "old@correo.com", "old_hash");
            var originalUpdatedAt = user.UpdatedAt;
            var originalCreatedAt = user.CreatedAt;
            
            string newUsername = "new_user_updated";
            string newEmail = "new@correo.com";
            string newHash = "new_super_secret_hash";
            
            System.Threading.Thread.Sleep(10);
            
            user.UpdateProfile(newUsername, newEmail, newHash);
            
            Assert.Equal(newUsername, user.Username);
            Assert.Equal(newEmail, user.Email);
            Assert.Equal(newHash, user.PasswordHash); 
            
            Assert.True(user.UpdatedAt > originalUpdatedAt);
            Assert.Equal(originalCreatedAt, user.CreatedAt);
        }

        [Fact]
        public void UpdateProfile_WithNullPasswordHash_ShouldKeepOldPasswordHash()
        {
            string originalHash = "old_hash_que_no_debe_cambiar";
            var user = new User("old_user", "old@correo.com", originalHash);
            var originalUpdatedAt = user.UpdatedAt;
            
            string newUsername = "new_user_updated";
            string newEmail = "new@correo.com";

            System.Threading.Thread.Sleep(10);
            
            user.UpdateProfile(newUsername, newEmail, null);
            
            Assert.Equal(newUsername, user.Username);
            Assert.Equal(newEmail, user.Email);
            
            Assert.Equal(originalHash, user.PasswordHash); 
            
            Assert.True(user.UpdatedAt > originalUpdatedAt);
        }
    }
}