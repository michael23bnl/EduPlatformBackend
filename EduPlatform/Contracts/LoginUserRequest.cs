using System.ComponentModel.DataAnnotations;

namespace EduPlatform.API.Contracts {
    public record LoginUserRequest(
        [Required] string Email,
        [Required] string Password);
}
