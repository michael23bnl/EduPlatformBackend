using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EduPlatform.Core.Models;
using EduPlatform.Persistence.Entities;
using EduPlatform.Core.Abstractions;
namespace EduPlatform.Persistence.Repositories {
    public class TasksRepository : ITasksRepository {

        private readonly EduPlatformDbContext _context;

        public TasksRepository(EduPlatformDbContext context) {
            _context = context;
        }

        public async Task<Guid> Create(TaskModel task) {
            var taskEntity = new TaskEntity {
                Id = task.Id,
                Theme = task.Theme,
                Content = task.Content,
                AnswerOptions = task.AnswerOptions,
                RightAnswer = task.RightAnswer,
            };
            await _context.Tasks.AddAsync(taskEntity);
            await _context.SaveChangesAsync();

            return taskEntity.Id;
        }

        public async Task<List<TaskModel>> GetAll() {

            var taskEntities = await _context.Tasks
                .AsNoTracking()
                .ToListAsync();

            var tasks = taskEntities
                .Select(t => TaskModel.Create(t.Id, t.Theme, t.Content, t.AnswerOptions, t.RightAnswer).task)
                .ToList();
            return tasks;
        }

        public async Task<TaskModel> Get(Guid id) {

            var taskEntity = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id);

            var task = TaskModel.Create(taskEntity.Id, taskEntity.Theme, taskEntity.Content, taskEntity.AnswerOptions, taskEntity.RightAnswer).task;

            return task;
        }

        public async Task<Guid> Update(Guid id, string theme, string content, List<string> answerOptions, string rightAnswer) {
            var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id);
            if (task != null) {
                task.Theme = theme;
                task.Content = content;
                task.AnswerOptions = answerOptions;
                task.RightAnswer = rightAnswer;
            }
            await _context.SaveChangesAsync();
            return id;
        }

        public async Task<Guid> Delete(Guid id) {
            var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id);
            if (task != null) {
                _context.Tasks.Remove(task);
                await _context.SaveChangesAsync();
            }
            return id;
        }

        public async Task<List<TaskModel>> GetByTheme(string theme) {
            var tasks = await _context.Tasks
                .Where(t => t.Theme == theme)
                .Select(t => TaskModel.Create(t.Id, t.Theme, t.Content, t.AnswerOptions, t.RightAnswer).task)
                .ToListAsync();
            return tasks;

        }

        public async Task<List<TaskModel>> GetIncorrectlySolvedByTheme(Guid userId, string theme) {
            var incorrectTasks = await (from utr in _context.UserTaskResults
                                         join t in _context.Tasks on utr.TaskId equals t.Id
                                         where t.Theme == theme && !utr.IsCorrect && utr.UserId == userId
                                         select TaskModel.Create(t.Id, theme, t.Content, t.AnswerOptions, t.RightAnswer).task).ToListAsync();
            return incorrectTasks;
        }
    }
}
