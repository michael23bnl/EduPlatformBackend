namespace EduPlatform.API.Contracts {
    public record TasksRequest(
        string theme,
        string content,
        List<string> answerOptions,
        string rightAnswer);
}
