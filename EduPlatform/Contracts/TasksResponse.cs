namespace EduPlatform.API.Contracts {
    public record TasksResponse(
        Guid id,
        string theme,
        string content,
        List<string> answerOptions,
        string rightAnswer);
}
