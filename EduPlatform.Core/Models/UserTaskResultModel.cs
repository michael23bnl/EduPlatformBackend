using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduPlatform.Core.Models {
    public class UserTaskResultModel {

        public UserTaskResultModel(Guid id, Guid userId, Guid taskId, bool isCorrect) {
            Id = id;
            UserId = userId;
            TaskId = taskId;
            IsCorrect = isCorrect;
        }

        public Guid Id { get; }

        public Guid UserId { get; }

        public Guid TaskId { get; }

        public bool IsCorrect { get; }
    }
}
