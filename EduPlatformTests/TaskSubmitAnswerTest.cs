using EduPlatform.API.Contracts;
using EduPlatform.API.Controllers;
using EduPlatform.Core.Abstractions;
using EduPlatform.Core.Models;
using EduPlatform.Persistence.Repositories;
using EduPlatform.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;
using EduPlatform.Persistence.Entities;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EduPlatformTests {
    public class TaskSubmitAnswerTest {
        [Fact]
        public async Task SubmitAnswer_TaskNotFound_ReturnsNotFound() { 
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

            mockTaskService.Setup(service => service.GetTask(It.IsAny<Guid>()))
            .ReturnsAsync(() => null);

            var controller = new TasksController(mockTaskService.Object, mockDbContext.Object) {
                ControllerContext = new ControllerContext {
                    HttpContext = mockHttpContext.Object
                }
            };

            var result = await controller.SubmitAnswer(Guid.NewGuid(), new SubmitAnswerRequest("Option1"));

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal("Задача не найдена", notFoundResult.Value);
        }

        [Fact]
        public async Task SubmitAnswer_EmptyAnswer_ReturnsBadRequest() {
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

            var controller = new TasksController(mockTaskService.Object, mockDbContext.Object) {
                ControllerContext = new ControllerContext {
                    HttpContext = mockHttpContext.Object
                }
            };

            var task = TaskModel.Create(
                    Guid.NewGuid(),
                    "Theme1",
                    "Content1",
                    new List<string> { "Option1", "Option2" },
                    "Option1"
                ).task!;

            mockTaskService.Setup(service => service.GetTask(It.IsAny<Guid>()))
                .ReturnsAsync(task);

            SubmitAnswerRequest request = new SubmitAnswerRequest(string.Empty);

            var result = await controller.SubmitAnswer(Guid.NewGuid(), request);
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Введите ответ", badRequestResult.Value);

        }
        [Fact]
        public async Task SubmitAnswer_TaskExistsAndCorrectAnswer_AddsResultAndReturnsOk() {
            var options = new DbContextOptionsBuilder<EduPlatformDbContext>()
        .UseInMemoryDatabase(databaseName: "EduPlatformMockDb")
        .Options;

            // Создание фейковых настроек авторизации
            var authOptions = Options.Create(new AuthorizationOptions {
                RolePermissions = new RolePermissions[0]
            });

            using (var mockDbContext = new EduPlatformDbContext(options, authOptions)) {
                var mockTaskService = new Mock<ITasksService>();
                var mockHttpContext = new Mock<HttpContext>();

                Guid userId = Guid.NewGuid();

                var userClaims = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] {
            new Claim("userId", userId.ToString()) 
        }));

                mockHttpContext.Setup(c => c.User).Returns(userClaims);

                // Создание задачи
                TaskModel task = TaskModel.Create(
                    Guid.NewGuid(),
                    "Theme1",
                    "Content1",
                    new List<string> { "Option1", "Option2" },
                    "Option1"
                ).task!;

                mockTaskService.Setup(service => service.GetTask(It.IsAny<Guid>()))
                    .ReturnsAsync(task);

                // Создание результата задачи пользователя и добавление его в контекст
                var userTaskResult = new UserTaskResultEntity {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    TaskId = task.Id,
                    IsCorrect = false
                };

                mockDbContext.UserTaskResults.Add(userTaskResult);
                await mockDbContext.SaveChangesAsync();

                // Создание контроллера
                var controller = new TasksController(mockTaskService.Object, mockDbContext) {
                    ControllerContext = new ControllerContext {
                        HttpContext = mockHttpContext.Object
                    }
                };

                var request = new SubmitAnswerRequest("Option1");

                // Выполнение метода
                var result = await controller.SubmitAnswer(task.Id, request);

                // Проверка результата
                var okResult = Assert.IsType<OkObjectResult>(result.Result);
                var response = Assert.IsType<SubmitAnswerResponse>(okResult.Value);
                Assert.Equal(true, response.IsCorrect);
            }
        }
        [Fact]
        public async Task SubmitAnswer_TaskExistsAndInсorrectAnswer_AddsResultAndReturnsOk() {
            var options = new DbContextOptionsBuilder<EduPlatformDbContext>()
        .UseInMemoryDatabase(databaseName: "EduPlatformMockDb")
        .Options;

            // Создание фейковых настроек авторизации
            var authOptions = Options.Create(new AuthorizationOptions {
                RolePermissions = new RolePermissions[0]
            });

            using (var mockDbContext = new EduPlatformDbContext(options, authOptions)) {
                var mockTaskService = new Mock<ITasksService>();
                var mockHttpContext = new Mock<HttpContext>();

                Guid userId = Guid.NewGuid();

                var userClaims = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] {
            new Claim("userId", userId.ToString()) 
        }));

                mockHttpContext.Setup(c => c.User).Returns(userClaims);

                // Создание задачи
                TaskModel task = TaskModel.Create(
                    Guid.NewGuid(),
                    "Theme1",
                    "Content1",
                    new List<string> { "Option1", "Option2" },
                    "Option1"
                ).task!;

                mockTaskService.Setup(service => service.GetTask(It.IsAny<Guid>()))
                    .ReturnsAsync(task);

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

                var userTaskResult = new UserTaskResultEntity {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    TaskId = task.Id,
                    IsCorrect = false
                };

                mockDbContext.UserTaskResults.Add(userTaskResult);
                await mockDbContext.SaveChangesAsync();

                var controller = new TasksController(mockTaskService.Object, mockDbContext) {
                    ControllerContext = new ControllerContext {
                        HttpContext = mockHttpContext.Object
                    }
                };

                var request = new SubmitAnswerRequest("IncorrectOption");

                var result = await controller.SubmitAnswer(task.Id, request);

                var okResult = Assert.IsType<OkObjectResult>(result.Result);
                var response = Assert.IsType<SubmitAnswerResponse>(okResult.Value);

                Assert.Equal(false, response.IsCorrect);
            }
        }
    }
}