using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EduPlatform.Core.Abstractions;
using EduPlatform.Core.Models;
using EduPlatform.Persistence.Repositories;

namespace EduPlatform.Application.Services {
    public class TasksService : ITasksService {

        private readonly ITasksRepository _tasksRepository;

        public TasksService(ITasksRepository tasksRepository) { 
            _tasksRepository = tasksRepository;
        }

        public async Task<List<TaskModel>> GetAllTasks() {
            return await _tasksRepository.GetAll();
        }

        public async Task<TaskModel> GetTask(Guid id) {
            return await _tasksRepository.Get(id);
        }

        public async Task<Guid> CreateTask(TaskModel task) {
            return await _tasksRepository.Create(task);
        }

        public async Task<Guid> UpdateTask(Guid id, string theme, string content, List<string> answerOptions, string rightAnswer) {
            return await _tasksRepository.Update(id, theme, content, answerOptions, rightAnswer);
        }

        public async Task<Guid> DeleteTask(Guid id) {
            return await _tasksRepository.Delete(id);
        }

        public async Task<List<TaskModel>> GetTasksByTheme(string theme) {
            return await _tasksRepository.GetByTheme(theme);
        }

        public async Task<List<TaskModel>> GetIncorrectlySolvedTasksByTheme(Guid userId, string theme) {
            return await _tasksRepository.GetIncorrectlySolvedByTheme(userId, theme);
        }

    }
}
