using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EduPlatform.Core.Abstractions;

namespace EduPlatform.Infrastructure {
    public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirenment> {

        private readonly IServiceScopeFactory _serviceScopeFactory;

        public PermissionAuthorizationHandler(IServiceScopeFactory serviceScopeFactory) {
            _serviceScopeFactory = serviceScopeFactory;
        }
        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context, 
            PermissionRequirenment requirement) {
            var userId = context.User.Claims.FirstOrDefault(
                c => c.Type == CustomClaims.UserId);
            if (userId is null || !Guid.TryParse(userId.Value, out var id)) {
                return;
            }
            using var scope = _serviceScopeFactory.CreateScope();

            var permissionService = scope.ServiceProvider.GetRequiredService<IPermissionService>();

            var permission = await permissionService.GetPermissionsAsync(id);

            if(permission.Intersect(requirement.Permissions).Any()) {
                context.Succeed(requirement);
            }
        }
    }
}
