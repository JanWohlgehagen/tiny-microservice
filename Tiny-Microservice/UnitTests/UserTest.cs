using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using SharedModels;
using System.Reflection;
using User.Controllers;
using User.Data;
using User.Data.repo;
using User.Helpers;
using User.PubSub;

namespace UnitTests
{
    public class UserTest
    {
        
        
        [Fact]
        public async Task Post_ValidUser_ReturnsOk()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<UserController>>();
            var repoMock = new Mock<IRepo>();
            var users = new List<User.Models.User>{}.AsQueryable();
            var userConverterMock = new Mock<IUserConverter>();
            var pubServiceMock = new Mock<IPubService>();
            var userSettingsConverterMock = new Mock<IUserSettingsConverter>();

            var userController = new UserController(loggerMock.Object, repoMock.Object, userConverterMock.Object, pubServiceMock.Object, userSettingsConverterMock.Object);

            var userFullDTO = new UserFullDTO
            {
                id = "1",
                name = "John Doe",
                imageURL = "https://example.com/image.jpg",
                email = "john@example.com",
                phoneNumber = "1234567890",
                address = "123 Main St",
                city = "Anytown",
                country = "Country",
                postalCode = "12345",
                about = "Lorem ipsum dolor sit amet, consectetur adipiscing elit."
            };

            

            var newUserSettings = new User.Models.UserSettings
            {
                id = "1",
                userId = "1",
                theme = "dark",
                language = "English"
            };
            var newUser = new User.Models.User
            {
                id = "1",
                name = "John Doe",
                imageURL = "https://example.com/image.jpg",
                email = "john@example.com",
                phoneNumber = "1234567890",
                address = "123 Main St",
                city = "Anytown",
                country = "Country",
                postalCode = "12345",
                about = "Lorem ipsum dolor sit amet, consectetur adipiscing elit.",
                settings = newUserSettings
            };

            userConverterMock.Setup(m => m.ConvertToNewUser(userFullDTO)).Returns(newUser);
            userSettingsConverterMock.Setup(m => m.ConvertToNewUserSettings(newUser.id)).Returns(newUserSettings);
            userConverterMock.Setup(m => m.ConvertToUserFullDTO(newUser)).Returns(userFullDTO);

            // Setup repo mock to return mock data when called
            var dbSetMock = new Mock<DbSet<User.Models.User>>();
            dbSetMock.As<IQueryable<User.Models.User>>().Setup(m => m.Provider).Returns(users.Provider);
            dbSetMock.As<IQueryable<User.Models.User>>().Setup(m => m.Expression).Returns(users.Expression);
            dbSetMock.As<IQueryable<User.Models.User>>().Setup(m => m.ElementType).Returns(users.ElementType);
            dbSetMock.As<IQueryable<User.Models.User>>().Setup(m => m.GetEnumerator()).Returns(users.GetEnumerator());

            repoMock.Setup(m => m.Users()).Returns(dbSetMock.Object);

            // Act
            var result = await userController.Post(userFullDTO) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.IsType<UserFullDTO>(result.Value);
        }

    }
}
