using EduPlatform.API.Contracts;
using EduPlatform.API.Controllers;
using EduPlatform.Application.Services;
using EduPlatform.Core.Abstractions;
using EduPlatform.Core.Models;
using EduPlatform.Persistence;
using EduPlatform.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using EduPlatform.Persistence.Repositories;
using Moq;
using Xunit;
using Microsoft.AspNetCore.Mvc;

namespace EduPlatformTests {
    public class TaskUpdateTest {
        [Fact]
        public async Task UpdateTask_ReturnsOkResult_WithTaskId() {
            var mockTaskService = new Mock<ITasksService>();
            var mockTaskRepository = new Mock<ITasksRepository>();

            // Создание in-memory контекста базы данных
            var options = new DbContextOptionsBuilder<EduPlatformDbContext>()
                .UseInMemoryDatabase(databaseName: "EduPlatformTestDb")
                .Options;

            // Создание фейковых настроек авторизации
            var authOptions = Options.Create(new AuthorizationOptions {
                RolePermissions = new RolePermissions[0]
            });

            var mockDbContext = new Mock<EduPlatformDbContext>(options, authOptions);

            Guid updatedTaskId = Guid.NewGuid();

            mockTaskService.Setup(service => service.UpdateTask(
                It.IsAny<Guid>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<List<string>>(),
                It.IsAny<string>()))
            .ReturnsAsync(updatedTaskId);

            var controller = new TasksController(mockTaskService.Object, mockDbContext.Object);

            var request = new TasksRequest(
                "NewTheme1",
                "NewContent1",
                new List<string> { "NewOption1", "NewOption2" },
                "NewOption1");

            var result = await controller.UpdateTask(Guid.NewGuid(), request);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<Guid>(okResult.Value);
            Assert.Equal(returnValue, updatedTaskId); // Проверяем количество задач 
        }
    }
}