

using EduPlatform.Application.Services;
using EduPlatform.API.Contracts;

namespace EduPlatform.API.Endpoints {
    public static class UsersEndpoints {
        public static IEndpointRouteBuilder MapUsersEndpoints(this IEndpointRouteBuilder app) {
            app.MapPost("register", Register);
            app.MapPost("login", Login);
            app.MapPost("logout", Logout);
            return app;
        }
        private static async Task<IResult> Register(RegisterUserRequest request, UsersService usersService) {
            await usersService.Register(request.UserName, request.Email, request.Password);
            return Results.Ok();
        }

        private static async Task<IResult> Login(LoginUserRequest request, UsersService usersService, HttpContext context) {

            var token = await usersService.Login(request.Email, request.Password);
            context.Response.Cookies.Append("suchatastycookie", token);
            return Results.Ok();
        }

        private static IResult Logout(HttpContext context) {
            if (context.Request.Cookies.ContainsKey("suchatastycookie")) {
                context.Response.Cookies.Delete("suchatastycookie");

                return Results.Ok();
            }
            else {
                return Results.NotFound();
            }
        }
    }
}
