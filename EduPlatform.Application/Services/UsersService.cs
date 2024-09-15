using EduPlatform.Core.Abstractions;
using EduPlatform.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduPlatform.Application.Services {
    public class UsersService : IUsersService {

        private readonly IPasswordHasher _passwordHasher;
        private readonly IUsersRepository _usersRepository;
        private readonly IJwtProvider _jwtProvider;

        public UsersService(IUsersRepository usersRepository, IPasswordHasher passwordHasher, IJwtProvider jwtProvider) {
            _passwordHasher = passwordHasher;
            _usersRepository = usersRepository;
            _jwtProvider = jwtProvider;
        }

        public async Task Register(string userName, string email, string password) {
            var hashedPassword = _passwordHasher.Generate(password);
            var userModel = UserModel.Create(Guid.NewGuid(), userName, hashedPassword, email);
            await _usersRepository.Create(userModel);
        }

        public async Task<string> Login(string email, string password) {
            var user = await _usersRepository.GetByEmail(email);
            var result = _passwordHasher.Verify(password, user.PasswordHash);
            if (result == false) {
                throw new Exception("Failed to login");
            }
            var token = _jwtProvider.GenerateToken(user);
            return token;
        }

    }
}
