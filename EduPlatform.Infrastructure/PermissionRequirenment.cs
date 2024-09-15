using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using EduPlatform.Core.Enums;

namespace EduPlatform.Infrastructure {
    public class PermissionRequirenment : IAuthorizationRequirement {

        public PermissionRequirenment(Permission[] permissions) { 
            Permissions = permissions;
        }
        public Permission[] Permissions { get; set; } = [];
    }
}
