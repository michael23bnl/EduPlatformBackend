using EduPlatform.Core.Abstractions;
using EduPlatform.Core.Models;
using Microsoft.AspNetCore.Mvc;
using EduPlatform.API.Contracts;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using EduPlatform.Persistence;
using EduPlatform.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace EduPlatform.API.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class TasksController : ControllerBase {

        private readonly ITasksService _taskService;
        private readonly EduPlatformDbContext _context;

        public TasksController(ITasksService tasksService, EduPlatformDbContext context) {
            _taskService = tasksService;
            _context = context;
        }

        //[Authorize(Policy = "Read")]
        [HttpGet("GetTasks")]
        public async Task<ActionResult<List<TasksResponse>>> GetTasks() {
            var tasks = await _taskService.GetAllTasks();

            var response = tasks.Select(t => new TasksResponse(t.Id, t.Theme, t.Content, t.AnswerOptions, t.RightAnswer)).ToList();

            return Ok(response);
        }
        [Authorize(Policy = "Create")]
        [HttpPost("CreateTask")]
        public async Task<ActionResult<Guid>> CreateTask([FromBody] TasksRequest request) {

            var (task, response) = TaskModel.Create(
                Guid.NewGuid(),
                request.theme,
                request.content,
                request.answerOptions,
                request.rightAnswer);

            if (response != "Task has been created") {
                return BadRequest(response);
            }

            var taskId = await _taskService.CreateTask(task);

            return Ok(taskId);
        }
        [Authorize(Policy = "Update")]
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<Guid>> UpdateTask(Guid id, [FromBody] TasksRequest request) {

            var (satisfy, response) = TaskModel.CheckProperties(request.theme, request.content, request.answerOptions, request.rightAnswer);
            if (!satisfy) {
                return BadRequest(response);
            }
            var taskId = await _taskService.UpdateTask(id, request.theme, request.content, request.answerOptions, request.rightAnswer);
            return Ok(taskId);
        }

        [Authorize(Policy = "Delete")]

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<Guid>> DeleteTask(Guid id) {
            return Ok(await _taskService.DeleteTask(id));
        }

        [Authorize(Policy = "Read")]

        [HttpGet("GetTasksByTheme")]
        public async Task<ActionResult<List<TasksResponse>>> GetTasksByTheme(string theme) {
            var tasks = await _taskService.GetTasksByTheme(theme);

            var response = tasks.Select(t => new TasksResponse(t.Id, t.Theme, t.Content, t.AnswerOptions, t.RightAnswer));

            return Ok(response);
        }
        [Authorize(Policy = "Read")]
        [HttpPost("{id:guid}")]
        public async Task<ActionResult<SubmitAnswerResponse>> SubmitAnswer(Guid id, [FromBody] SubmitAnswerRequest request) {
            var task = await _taskService.GetTask(id);
            if (task == null) {
                return NotFound("Задача не найдена");
            }

            if (string.IsNullOrEmpty(request.SelectedAnswer)) {
                return BadRequest("Введите ответ");
            }

            bool isCorrect = task.RightAnswer == request.SelectedAnswer;

            // здесь userId гарантированно не будет равен null, т.к.
            // метод доступен только авторизованным пользователям

            var userId = User.FindFirst("userId")!.Value;

            // нужно проверить, решал ли уже пользователь эту задачу  

            var userTaskResult = await _context.UserTaskResults
            .FirstOrDefaultAsync(utr => utr.UserId == Guid.Parse(userId) && utr.TaskId == id);

            // если нет, то добавляем результат решения в БД

            if (userTaskResult == null) {
                userTaskResult = new UserTaskResultEntity {
                    Id = Guid.NewGuid(),
                    UserId = Guid.Parse(userId),
                    TaskId = id,
                    IsCorrect = isCorrect,
                };
                _context.UserTaskResults.Add(userTaskResult);
            }

            // если результат в БД уже имеется, то перезаписываем показатель правильности решения

            else {
                userTaskResult.IsCorrect = isCorrect;
            }
          
            // вне зависимости от итогов предыдущей проверки сохраняем результат в БД

            await _context.SaveChangesAsync();

            bool showRecommendations = false;

            var theme = task.Theme;

            if (!isCorrect) {
                var tasks = await _taskService.GetIncorrectlySolvedTasksByTheme(Guid.Parse(userId), theme);
                if (tasks.Count() > 5) {
                    showRecommendations = true;              
                }

            }

            return Ok(new SubmitAnswerResponse(isCorrect, showRecommendations, theme));
        }
        [Authorize(Policy = "Read")]

        [HttpGet("GetIncorrectlySolvedTasksByTheme")]
        public async Task<ActionResult<List<TasksResponse>>> GetIncorrectlySolvedTasksByTheme(string theme) {
            var userId = User.FindFirst("userId")!.Value;           

            var tasks = await _taskService.GetIncorrectlySolvedTasksByTheme(Guid.Parse(userId), theme);

            var response = tasks.Select(t => new TasksResponse(t.Id, t.Theme, t.Content, t.AnswerOptions, t.RightAnswer)).ToList();

            return Ok(response);
        }

    }
}
