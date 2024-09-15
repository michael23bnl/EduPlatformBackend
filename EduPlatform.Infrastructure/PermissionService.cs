using EduPlatform.Core.Abstractions;
using EduPlatform.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduPlatform.Infrastructure {
    public class PermissionService : IPermissionService {
        private readonly IUsersRepository _usersRepository;
        public PermissionService(IUsersRepository usersRepository) {
            _usersRepository = usersRepository;
        }
        public Task<HashSet<Permission>> GetPermissionsAsync(Guid userId) {
            return _usersRepository.GetUserPermissions(userId);
        }
    }
}
