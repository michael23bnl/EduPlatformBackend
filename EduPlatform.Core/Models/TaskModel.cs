using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduPlatform.Core.Models {
    public class TaskModel {

        private TaskModel(Guid id, string theme, string content, List<string> answerOptions, string rightAnswer) {
            Id = id;
            Theme = theme;
            Content = content;
            AnswerOptions = answerOptions;
            RightAnswer = rightAnswer;
        }

        public Guid Id { get; }

        public string Theme { get; } = string.Empty;

        public string Content { get; } = string.Empty;

        public List<string> AnswerOptions { get; }

        public string RightAnswer { get; } = string.Empty;

        public static (bool satisfy, string response) CheckProperties(string theme, string content, List<string> answerOptions, string rightAnswer) {
            string response = "Task's properties do satisfy the requirements";
            if (string.IsNullOrEmpty(theme)) {
                response = "Task must have a theme";
                return (false, response);
            }
            if (string.IsNullOrEmpty(content)) {
                response = "Task must have a content";
                return (false, response);
            }
            /*
            if (answerOptions.Count < 2) { // проверка, содержит ли задание более 1 варианта ответа
                response = "Task must have more than 1 answer options";
                return (false, response);
            }
            if (string.IsNullOrEmpty(rightAnswer) || !answerOptions.Contains(rightAnswer)) { // проверка, есть ли в списке ответов правильный ответ
                response = "Task must have a right answer";
                return (false, response);
            }
            */
            return (true, response);
        }

        public static (TaskModel? task, string response) Create(Guid id, string theme, string content, List<string> answerOptions, string rightAnswer) {
            (bool satisfy, string response) = CheckProperties(theme, content, answerOptions, rightAnswer);
            if (satisfy) {
                var task = new TaskModel(id, theme, content, answerOptions, rightAnswer);
                response = "Task has been created";
                return (task, response);
            }       
            return (null, response);
        }

    }
}
