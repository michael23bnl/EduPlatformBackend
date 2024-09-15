using EduPlatform.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Permission = EduPlatform.Core.Enums.Permission;
using Role = EduPlatform.Core.Enums.Role;

namespace EduPlatform.Persistence.Configurations {
    public partial class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermissionEntity> {


        private readonly AuthorizationOptions _authorizationOptions;

        public RolePermissionConfiguration(AuthorizationOptions authorizationOptions) {
            _authorizationOptions = authorizationOptions;
        }

        public void Configure(EntityTypeBuilder<RolePermissionEntity> builder) {

            builder.HasKey(r => new { r.RoleId, r.PermissionId });
            var rolePermissions = ParseRolePermissions();
            builder.HasData(rolePermissions);

        }

        private List<RolePermissionEntity> ParseRolePermissions() {

            return _authorizationOptions.RolePermissions
                .SelectMany(rp => rp.Permissions
                    .Select(p => new RolePermissionEntity {
                        RoleId = (int)Enum.Parse<Role>(rp.Role),
                        PermissionId = (int)Enum.Parse<Permission>(p)
                    }))
                .ToList();
        }
    }
}
