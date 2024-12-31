using EduPlatform.API.Contracts;
using EduPlatform.API.Controllers;
using EduPlatform.Core.Abstractions;
using EduPlatform.Core.Models;
using EduPlatform.Persistence;
using EduPlatform.Persistence.Entities;
using EduPlatform.Persistence.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace EduPlatformTests {
    public class TaskDeleteTest {
        [Fact]
        public async Task DeleteTask_ReturnsOkResult_WithTaskId() {
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
      
            Guid removedTaskId = Guid.NewGuid();
            mockTaskService.Setup(service => service.DeleteTask(It.IsAny<Guid>()))
                .ReturnsAsync(removedTaskId);

            var controller = new TasksController(mockTaskService.Object, mockDbContext.Object);

            var result = await controller.DeleteTask(Guid.NewGuid());

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<Guid>(okResult.Value);
            Assert.Equal(returnValue, removedTaskId); // Проверяем количество задач
        }
    }
}