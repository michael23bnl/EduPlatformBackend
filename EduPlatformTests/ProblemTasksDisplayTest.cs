using EduPlatform.API.Contracts;
using EduPlatform.API.Controllers;
using EduPlatform.Core.Abstractions;
using EduPlatform.Core.Models;
using EduPlatform.Persistence.Entities;
using EduPlatform.Persistence.Repositories;
using EduPlatform.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace EduPlatformTests {
    public class ProblemTaskDisplayTest {
        [Fact]
        public async Task DisplayIncorrects_ReturnsOkResult_WithListOfTasksResponse() {
            var mockTaskService = new Mock<ITasksService>();
            var mockHttpContext = new Mock<HttpContext>();
            // Создание in-memory контекста базы данных
            var options = new DbContextOptionsBuilder<EduPlatformDbContext>()
                .UseInMemoryDatabase(databaseName: "EduPlatformMockDb")
                .Options;

            // Создание фейковых настроек авторизации
            var authOptions = Options.Create(new AuthorizationOptions {
                RolePermissions = new RolePermissions[0]
            });

            var mockDbContext = new Mock<EduPlatformDbContext>(options, authOptions);

            var userClaims = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim("userId", Guid.NewGuid().ToString())
            }));

            mockHttpContext.Setup(c => c.User).Returns(userClaims);

            var incorrects = new List<TaskModel> {
                TaskModel.Create(
                    Guid.NewGuid(),
                    "Theme1",
                    "Content1",
                    new List<string> { "Option1", "Option2" },
                    "Option1"
                ).task!,
                TaskModel.Create(
                    Guid.NewGuid(),
                    "Theme1",
                    "Content2",
                    new List<string> { "Option1", "Option2", "Option3" },
                    "Option2"
                ).task!,
            };

            mockTaskService.Setup(service => service.GetIncorrectlySolvedTasksByTheme(It.IsAny<Guid>(), It.IsAny<string>()))          
            .ReturnsAsync(incorrects);

            var controller = new TasksController(mockTaskService.Object, mockDbContext.Object) {
                ControllerContext = new ControllerContext {
                    HttpContext = mockHttpContext.Object 
                }
            };

            var result = await controller.GetIncorrectlySolvedTasksByTheme("Theme1");

            var okResult = Assert.IsType<OkObjectResult>(result.Result);

            var returnValue = Assert.IsType<List<TasksResponse>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);

        }
    }
}