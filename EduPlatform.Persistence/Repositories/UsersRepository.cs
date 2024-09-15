using EduPlatform.Core.Models;
using EduPlatform.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EduPlatform.Core.Abstractions;
using EduPlatform.Core.Enums;

namespace EduPlatform.Persistence.Repositories {
    public class UsersRepository : IUsersRepository {

        private readonly EduPlatformDbContext _context;


        public UsersRepository(EduPlatformDbContext context) { 
            _context = context;
        }

        public async Task Create(UserModel userModel) {
            
            var roleEntity = await _context.Roles
                .SingleOrDefaultAsync(r => r.Id == (int)Role.Admin)
                ?? throw new InvalidOperationException();

            var userEntity = new UserEntity() {
                Id = userModel.Id,
                UserName = userModel.UserName,
                PasswordHash = userModel.PasswordHash,
                Email = userModel.Email,
                Roles = [roleEntity]
            };
            await _context.Users.AddAsync(userEntity);
            await _context.SaveChangesAsync();
        }

        public async Task<UserModel> GetByEmail(string email) {

            var userEntity = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == email) ?? throw new Exception();

            return UserModel.Create(userEntity.Id, userEntity.UserName, userEntity.PasswordHash, userEntity.Email);

        }

        public async Task<HashSet<Permission>> GetUserPermissions(Guid userId) {
            var roles = await _context.Users
                .AsNoTracking()
                .Include(u => u.Roles)
                .ThenInclude(r => r.Permissions)
                .Where(u => u.Id == userId)
                .Select(u => u.Roles)
                .ToListAsync();

            return roles
                .SelectMany(r => r)
                .SelectMany(r => r.Permissions)
                .Select(p => (Permission)p.Id)
                .ToHashSet();
        }

    }
}
