using EduPlatform.API.Contracts;
using EduPlatform.API.Controllers;
using EduPlatform.Core.Abstractions;
using EduPlatform.Core.Models;
using EduPlatform.Persistence.Entities;
using EduPlatform.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;
using System.Threading.Tasks;
using EduPlatform.Persistence.Repositories;
namespace EduPlatformTests {
    public class TaskCreateTest {
        [Fact]
        public async Task CreateTask_ReturnsOkResult_WithTaskId() {
            var mockTaskService = new Mock<ITasksService>();

            // Создание in-memory контекста базы данных
            var options = new DbContextOptionsBuilder<EduPlatformDbContext>()
                .UseInMemoryDatabase(databaseName: "EduPlatformTestDb")
                .Options;

            // Создание фейковых настроек авторизации
            var authOptions = Options.Create(new AuthorizationOptions {
                RolePermissions = new RolePermissions[0]
            });

            var mockDbContext = new Mock<EduPlatformDbContext>(options, authOptions);

            Guid createdTaskId = Guid.NewGuid();

            mockTaskService.Setup(service => service.CreateTask(It.IsAny<TaskModel>()))
            .ReturnsAsync(createdTaskId);  
            var controller = new TasksController(mockTaskService.Object, mockDbContext.Object);

            var request = new TasksRequest(
                "Theme1",
                "Content1",
                new List<string> { "Option1", "Option2" },
                "Option1");

            var result = await controller.CreateTask(request);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<Guid>(okResult.Value);
            Assert.Equal(returnValue, Guid.NewGuid()); // Проверяем количество задач
        }
    }
}