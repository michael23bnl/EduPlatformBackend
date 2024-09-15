using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduPlatform.Persistence.Entities {
    public class TaskEntity {

        public Guid Id { get; set; }

        public string Theme { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;

        public List<string> AnswerOptions { get; set; } = new List<string>();

        public string RightAnswer { get; set; } = string.Empty;

    }
}
