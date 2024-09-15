using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduPlatform.Persistence.Entities {
    public class UserTaskResultEntity {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public Guid TaskId { get; set; }

        public bool IsCorrect { get; set; }

    }
}
