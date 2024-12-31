using Xunit;
using Moq;
using EduPlatform.Core.Abstractions;
using EduPlatform.Core.Models;
using EduPlatform.API.Contracts;
using EduPlatform.API.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EduPlatform.Persistence.Entities;
using EduPlatform.Persistence;
using Xunit.Abstractions;
using EduPlatform.Persistence.Repositories;

namespace EduPlatformTests {
    public class TaskGetTest {

        [Fact]
        public async Task GetTasks_ReturnsOkResult_WithTasksList() {
            var mockTaskService = new Mock<ITasksService>();

            // Создание in-memory контекста базы данных
            var options = new DbContextOptionsBuilder<EduPlatformDbContext>()
                .UseInMemoryDatabase(databaseName: "EduPlatformMockDb")
                .Options;

            // Создание фейковых настроек авторизации
            var authOptions = Options.Create(new AuthorizationOptions {
                RolePermissions = new RolePermissions[0]
            });

            var mockDbContext = new Mock<EduPlatformDbContext>(options, authOptions);

            var tasks = new List<TaskModel> {
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

            mockTaskService.Setup(service => service.GetAllTasks())
                .ReturnsAsync(tasks);

            var controller = new TasksController(mockTaskService.Object, mockDbContext.Object);
            var result = await controller.GetTasks();

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.NotNull(okResult.Value); // Проверяем, что Value не null
            var returnValue = Assert.IsType<List<TasksResponse>>(okResult.Value);
            Assert.Equal(2, returnValue.Count); // Проверяем количество задач
        }
    }
}