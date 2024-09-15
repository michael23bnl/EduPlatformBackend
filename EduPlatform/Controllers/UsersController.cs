/*
using EduPlatform.Core.Abstractions;
using Microsoft.AspNetCore.Mvc;
using EduPlatform.API.Contracts;
using EduPlatform.Application.Services;


namespace EduPlatform.API.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase {

        private readonly IUsersService _userService;

        public UsersController(IUsersService usersService) { 
            _userService = usersService;
        }

        [HttpPost("register")]
        public async Task<IResult> Register([FromBody] RegisterUserRequest request) {
            await _userService.Register(request.UserName, request.Email, request.Password);
            return Results.Ok();
        }
        [HttpPost("login")]
        public async Task<IResult> Login([FromBody] LoginUserRequest request) {

            var token = await _userService.Login(request.Email, request.Password);
            return Results.Ok(token);
        }
    }
}
*/