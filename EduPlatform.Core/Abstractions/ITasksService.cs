using EduPlatform.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduPlatform.Core.Abstractions {
    public interface ITasksService {
        public Task<List<TaskModel>> GetAllTasks();

        public Task<TaskModel> GetTask(Guid id);

        public Task<Guid> CreateTask(TaskModel task);

        public Task<Guid> UpdateTask(Guid id, string theme, string content, List<string> answerOptions, string rightAnswer);

        public Task<Guid> DeleteTask(Guid id);

        public Task<List<TaskModel>> GetTasksByTheme(string theme);

        public Task<List<TaskModel>> GetIncorrectlySolvedTasksByTheme(Guid userId, string theme);
    }
}

