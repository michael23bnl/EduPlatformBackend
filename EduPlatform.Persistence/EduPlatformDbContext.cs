using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using EduPlatform.Persistence.Entities;
using Microsoft.Extensions.Options;
using EduPlatform.Persistence.Configurations;

namespace EduPlatform.Persistence {
    public class EduPlatformDbContext(DbContextOptions<EduPlatformDbContext> options,
        IOptions<AuthorizationOptions> authOptions) : DbContext(options) {

        /*public EduPlatformDbContext(DbContextOptions<EduPlatformDbContext> options) 
            : base(options) {
        }*/

        public DbSet<TaskEntity> Tasks { get; set; }

        public DbSet<UserEntity> Users { get; set; }

        public DbSet<UserTaskResultEntity> UserTaskResults { get; set; }

        public DbSet<RoleEntity> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(EduPlatformDbContext).Assembly);
            modelBuilder.ApplyConfiguration(new RolePermissionConfiguration(authOptions.Value));
        }
    }
}

