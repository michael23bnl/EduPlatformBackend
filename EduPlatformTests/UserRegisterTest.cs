using EduPlatform.API.Contracts;
using EduPlatform.API.Controllers;
using EduPlatform.Core.Abstractions;
using EduPlatform.Persistence.Entities;
using EduPlatform.Persistence;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;
using System.Threading.Channels;

namespace EduPlatformTests {
    public class UserRegisterTest {
        [Fact]
        public async Task RegisterUser_ReturnsOkResult() {

            var mockUserService = new Mock<IUsersService>();

            // Создание in-memory контекста базы данных
            var options = new DbContextOptionsBuilder<EduPlatformDbContext>()
                .UseInMemoryDatabase(databaseName: "EduPlatformTestDb")
                .Options;

            // Создание фейковых настроек авторизации
            var authOptions = Options.Create(new AuthorizationOptions {
                RolePermissions = new RolePermissions[0]
            });

            var mockDbContext = new Mock<EduPlatformDbContext>(options, authOptions);

            var userEntity1 = new UserEntity {
                Id = Guid.NewGuid(),
                UserName = "UserName1",
                PasswordHash = "PasswordHash1",
                Email = "Email1",
                Roles = new List<RoleEntity>()
            };

            mockUserService.Setup(service => service.Register(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Callback<string, string, string>((userName, email, password) => {

                });

            var request = new RegisterUserRequest("Name1", "Password1", "Email1");

            var controller = new UsersController(mockUserService.Object);

            var result = await controller.Register(request);

            var okResult = Assert.IsType<Ok>(result);
        }
    }
}