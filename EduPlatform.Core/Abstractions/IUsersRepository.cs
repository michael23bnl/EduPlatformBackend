using EduPlatform.Core.Enums;
using EduPlatform.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduPlatform.Core.Abstractions {
    public interface IUsersRepository {
        public Task Create(UserModel userModel);

        public Task<UserModel> GetByEmail(string email);

        public Task<HashSet<Permission>> GetUserPermissions(Guid userId);

    }
}
