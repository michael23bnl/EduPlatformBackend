using System.ComponentModel.DataAnnotations;

namespace EduPlatform.API.Contracts {
    public record RegisterUserRequest(
        [Required] string UserName,
        [Required] string Password,
        [Required] string Email);
}