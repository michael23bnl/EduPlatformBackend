namespace EduPlatform.API.Contracts {
    public record SubmitAnswerResponse(
        bool IsCorrect,
        bool showRecommendations,
        string theme);
}
