using EduPlatform.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduPlatform.Persistence.Configurations {
    public class TaskConfiguration : IEntityTypeConfiguration<TaskEntity> {
        public void Configure(EntityTypeBuilder<TaskEntity> builder) {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Theme).IsRequired();
            builder.Property(x => x.Content).IsRequired();
            builder.Property(x => x.AnswerOptions).IsRequired();
            builder.Property(x => x.RightAnswer).IsRequired();
        }
    }
}
