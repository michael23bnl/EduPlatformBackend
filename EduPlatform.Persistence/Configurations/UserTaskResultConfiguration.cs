using EduPlatform.Persistence.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduPlatform.Persistence.Configurations {
    public class UserTaskResultConfiguration : IEntityTypeConfiguration<UserTaskResultEntity> {
        public void Configure(EntityTypeBuilder<UserTaskResultEntity> builder) {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.UserId).IsRequired();
            builder.Property(x => x.TaskId).IsRequired();
            builder.Property(x => x.IsCorrect).IsRequired();
        }
    }
}
