using EduPlatform.Core.Enums;
using EduPlatform.Persistence.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduPlatform.Persistence.Configurations {
    public partial class PermissionConfiguration : IEntityTypeConfiguration<PermissionEntity> {
        public void Configure(EntityTypeBuilder<PermissionEntity> builder) {
            builder.HasKey(p => p.Id);

            var permissions = Enum
                .GetValues<Permission>()
                .Select(p => new PermissionEntity {
                    Id = (int)p,
                    Name = p.ToString()
                });

            builder.HasData(permissions);            
        }
    }
}



