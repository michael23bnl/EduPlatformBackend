using EduPlatform.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduPlatform.Core.Abstractions {
    public interface ITasksRepository {

        public Task<Guid> Create(TaskModel task);
        public Task<List<TaskModel>> GetAll();

        public Task<TaskModel> Get(Guid id);

        public Task<Guid> Update(Guid id, string theme, string content, List<string> answerOptions, string rightAnswer);

        public Task<Guid> Delete(Guid id);

        public Task<List<TaskModel>> GetByTheme(string theme);

        public Task<List<TaskModel>> GetIncorrectlySolvedByTheme(Guid userId, string theme);

    }
}
