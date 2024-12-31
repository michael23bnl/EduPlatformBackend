using EduPlatform.API.Contracts;
using EduPlatform.API.Controllers;
using EduPlatform.Core.Abstractions;
using EduPlatform.Persistence.Entities;
using EduPlatform.Persistence.Repositories;
using EduPlatform.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;
using EduPlatform.Core.Models;
using EduPlatform.API.Endpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace EduPlatformTests {
    public class UserLoginTest {
        [Fact]
        public async Task LoginUser_ReturnsOkResultWithToken() {

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

            mockUserService.Setup(service => service.Login(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync("token"
                );

            var request = new LoginUserRequest("Email1", "Password1");


            var controller = new UsersController(mockUserService.Object);
    
            var result = await controller.Login(request);

            var okResult = Assert.IsType<Ok<string>>(result);
            var returnValue = Assert.IsType<string>(okResult.Value);
            Assert.Equal(returnValue, "token");          
        }
    }
}