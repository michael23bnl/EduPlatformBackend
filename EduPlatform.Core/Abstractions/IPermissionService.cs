using EduPlatform.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduPlatform.Core.Abstractions {
    public interface IPermissionService {
        public Task<HashSet<Permission>> GetPermissionsAsync(Guid userId);
    }
}
